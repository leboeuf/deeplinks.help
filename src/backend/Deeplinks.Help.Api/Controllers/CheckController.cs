using Microsoft.AspNetCore.Mvc;

namespace Deeplinks.Help.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CheckController : ControllerBase
{
    private readonly ILogger<CheckController> _logger;

    public CheckController(ILogger<CheckController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
}
