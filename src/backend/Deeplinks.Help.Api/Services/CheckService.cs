using Deeplinks.Help.Api.Infrastructure.Constants;
using Deeplinks.Help.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Deeplinks.Help.Api.Models.Output;
using System.Net;

namespace Deeplinks.Help.Api.Services
{
    public class CheckService
    {
        private readonly ILogger<CheckService> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;

        public CheckService(
            ILogger<CheckService> logger,
            IMemoryCache memoryCache,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Fetches the Android App Links assetlinks.json file from the specified domain.
        /// </summary>
        public async Task<ServiceOutput> FetchAndroid(string domain)
        {
            // add caching to avoid multiple requests for the same domain
            // Assign a guid or hash to the domain upon the first request, and reuse it in the next requests so we don't have to validate the input every check
            var uri = Utils.CreateSafeUri(domain);
            if (uri == null)
            {
                _logger.LogWarning("Invalid URI provided: {domain}", domain);
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

            var file = new Uri($"https://{uri.Host}/.well-known/assetlinks.json");
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
