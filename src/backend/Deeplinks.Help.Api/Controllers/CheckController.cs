using Deeplinks.Help.Api.Infrastructure;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Deeplinks.Help.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CheckController : ControllerBase
{
    private readonly ILogger<CheckController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public CheckController(
        ILogger<CheckController> logger,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    [HttpPost]
    public async Task<IActionResult> FetchAndroid([FromBody] string input)
    {
        var uri = Utils.CreateSafeUri(input);
        var file = new Uri($"https://{uri.Host}/.well-known/assetlinks.json");
        using var httpClient = _httpClientFactory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Get, uri);
        var response = await httpClient.SendAsync(request);
        return Ok();
    }
}
