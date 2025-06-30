using Deeplinks.Help.Examples.Services;
using Microsoft.AspNetCore.Mvc;

namespace Deeplinks.Help.Examples.Controllers;

[ApiController]
[Route(".well-known")]
public class WellKnownController : ControllerBase
{
    private readonly ExampleService _exampleService;

    public WellKnownController(ExampleService exampleService)
    {
        _exampleService = exampleService;
    }

    [HttpGet("assetlinks.json")]
#if DEBUG
    public IActionResult GetAndroid([FromQuery] string subdomain)
    {
        return GetExample(subdomain, "assetlinks.json");
    }
#else
    public IActionResult GetAndroid()
    {
        var subdomain = Request.Host.Value?.Split('.').FirstOrDefault()?.ToLowerInvariant();
        return GetExample(subdomain, "assetlinks.json");
    }
#endif

    [HttpGet("apple-app-site-association")]
#if DEBUG
    public IActionResult GetApple([FromQuery] string subdomain)
    {
        return GetExample(subdomain, "apple-app-site-association");
    }
#else
    public IActionResult GetApple()
    {
        var subdomain = Request.Host.Value?.Split('.').FirstOrDefault()?.ToLowerInvariant();
        return GetExample(subdomain, "apple-app-site-association");
    }
#endif

    private IActionResult GetExample(string subdomain, string fileName)
    { 
        if (subdomain == null)
        {
            return NotFound();
        }

        if (subdomain.Length > 20)
        {
            // Avoid length attacks
            return BadRequest();
        }

        // Check if the subdomain matches one of the example files
        var isFileExample = Directory.Exists(Path.Combine("Examples", subdomain));
        if (isFileExample)
        {
            var content = System.IO.File.ReadAllText(Path.Combine("Examples", subdomain, fileName));
            return Content(content, "application/json");
        }

        // Check if the subdomain matches one of the example methods
        string[] examples = ["404", "bad-content-type"];
        if (examples.Contains(subdomain))
        {
            return _exampleService.GetExample(subdomain, fileName);
        }

        return NotFound();
    }
}
