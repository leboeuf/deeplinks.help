namespace Deeplinks.Help.Api.Models.Output
{
    public class ServiceOutput
    {
        public bool Success { get; set; }
        public object? Model { get; set; }
        public ErrorOutput? Error { get; set; }
    }
}
