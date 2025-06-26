using System.Net;

namespace Deeplinks.Help.Api.Models.Output
{
    public class ErrorOutput
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }
    }
}
