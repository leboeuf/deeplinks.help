namespace Deeplinks.Help.Api.Infrastructure.Constants
{
    public static class HttpClients
    {
        public const string ChecksHttpClient = nameof(ChecksHttpClient);
    }

    public static class HttpClientConfiguration
    {
        public static TimeSpan Timeout = TimeSpan.FromSeconds(10);
    }
}
