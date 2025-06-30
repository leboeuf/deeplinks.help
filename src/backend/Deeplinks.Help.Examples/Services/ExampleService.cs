using Microsoft.AspNetCore.Mvc;

namespace Deeplinks.Help.Examples.Services
{
    public class ExampleService
    {
        public IActionResult GetExample(string example, string fileName)
        {
            switch (example)
            {
                case "401":
                    return new UnauthorizedResult();
                case "403":
                    return new ForbidResult();
                case "404":
                    return new NotFoundResult();
                case "500":
                    return new StatusCodeResult(500);
                case "bad-content-type":
                    return new ContentResult
                    {
                        Content = File.ReadAllText(Path.Combine("Examples", "valid", fileName)),
                        ContentType = "text/plain"
                    };
                default:
                    break;
            }

            throw new ArgumentException($"Unknown example: {example}", nameof(example));
        }
    }
}
