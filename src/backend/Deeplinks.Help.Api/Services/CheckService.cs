﻿using Deeplinks.Help.Api.Infrastructure;
using Deeplinks.Help.Api.Infrastructure.Configuration;
using Deeplinks.Help.Api.Infrastructure.Constants;
using Deeplinks.Help.Api.Models;
using Deeplinks.Help.Api.Models.Output;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Net;

namespace Deeplinks.Help.Api.Services
{
    public class CheckService
    {
        private readonly ILogger<CheckService> _logger;
        private readonly TimeSpan _domainDataCacheDuration;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;

        public CheckService(
            ILogger<CheckService> logger,
            IOptions<CacheConfiguration> cacheConfiguration,
            IMemoryCache memoryCache,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;

            _domainDataCacheDuration = TimeSpan.FromSeconds(cacheConfiguration.Value.DomainDataCacheDurationInSeconds);
        }

        /// <summary>
        /// Validates the user-input domain. If valid, caches it and returns a hash
        /// that the client must provide in future requests to identify this domain.
        /// </summary>
        public ServiceOutput ValidateDomain(string domain)
        {
            var uri = Utils.CreateSafeUri(domain);
            if (uri == null)
            {
                return new ServiceOutput
                {
                    Success = false,
                    Error = new ErrorOutput
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "Invalid domain."
                    }
                };
            }

            var domainHash = Utils.Hash(uri.Host);
            var domainData = _memoryCache.GetOrCreate(domainHash, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _domainDataCacheDuration;
                return new DomainData { Domain = uri.Host };
            });

            return new ServiceOutput
            {
                Success = true,
                Model = domainHash
            };
        }

        /// <summary>
        /// Fetches the Android App Links assetlinks.json file from the specified domain.
        /// </summary>
        public async Task<ServiceOutput> FetchAndroid(string domainHash)
        {
            var domainData = _memoryCache.Get<DomainData>(domainHash);
            if (domainData == null)
            {
                return new ServiceOutput
                {
                    Success = false,
                    Error = new ErrorOutput
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Domain not found in cache."
                    }
                };
            }

            var file = new Uri($"https://{domainData.Domain}/.well-known/assetlinks.json");
            using var httpClient = _httpClientFactory.CreateClient(HttpClients.ChecksHttpClient);
            httpClient.Timeout = HttpClientConfiguration.Timeout;

            var request = new HttpRequestMessage(HttpMethod.Get, file);

            var response = await httpClient.SendAsync(request);

            var body = await response.Content.ReadAsStringAsync();
            return new ServiceOutput
            {
                Success = true,
                Model = body,
            };
        }
    }
}
