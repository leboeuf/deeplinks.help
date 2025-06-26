using Deeplinks.Help.Api.Infrastructure;
using Deeplinks.Help.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Deeplinks.Help.Api.Controllers;

[ApiController]
[Route("checks")]
public class CheckController : BaseApiController
{
    private readonly CheckService _checkService;

    public CheckController(CheckService checkService)
    {
        _checkService = checkService;
    }

    [HttpPost("fetch-android")]
    public async Task<IActionResult> FetchAndroid([FromBody] string input)
    {
        return ApiResult(await _checkService.FetchAndroid(input));
    }
}
