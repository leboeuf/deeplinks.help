using Deeplinks.Help.Api.Infrastructure;
using Deeplinks.Help.Api.Models.Input;
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

    [HttpPost("validate-domain")]
    public IActionResult ValidateDomain([FromBody] DomainInput input)
    {
        return ApiResult(_checkService.ValidateDomain(input.Domain));
    }

    [HttpPost("fetch-android")]
    public async Task<IActionResult> FetchAndroid([FromBody] HashInput input)
    {
        return ApiResult(await _checkService.FetchAndroid(input.Hash));
    }
}
