namespace Deeplinks.Help.Api.Models.Output
{
    public class CheckOutput
    {
        /// <summary>
        /// Indicates whether the processing of other checks should stop.
        /// </summary>
        public bool Stop { get; set; }

        /// <summary>
        /// The status of the verification process ("success", "error", "warning").
        /// </summary>
        public required string Status { get; set; }

        /// <summary>
        /// A brief message describing the result.
        /// </summary>
        public string? Msg { get; set; }

        /// <summary>
        /// Additional details providing guidance for the result.
        /// </summary>
        public string? Details { get; set; }

    }
}
