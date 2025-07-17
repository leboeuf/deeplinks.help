using Deeplinks.Help.Api.Models.Output;
using Microsoft.AspNetCore.Mvc;

namespace Deeplinks.Help.Api.Infrastructure
{
    public class BaseApiController : ControllerBase
    {
        /// <summary>
        /// Returns a standardized API response based on the given ServiceOutput.
        /// </summary>
        protected IActionResult ApiResult(ServiceOutput result)
        {
            if (result.Error != null)
            {
                return StatusCode((int)result.Error!.StatusCode, result.Error.Message);
            }

            return Ok(result.Model);
        }
    }
}
