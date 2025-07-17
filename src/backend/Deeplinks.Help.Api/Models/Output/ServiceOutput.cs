namespace Deeplinks.Help.Api.Models.Output
{
    public class ServiceOutput
    {
        /// <summary>
        /// The result of the request processing.
        /// If an error occurred, this will be null.
        /// </summary>
        public object? Model { get; set; }

        /// <summary>
        /// Any error that occurred during processing of the request.
        /// If no error occurred, this will be null.
        /// </summary>
        public ErrorOutput? Error { get; set; }
    }
}
