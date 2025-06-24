using Deeplinks.Help.Api.Infrastructure;

namespace Deeplinks.Help.Tests.Unit
{
    public class UriTests
    {
        [Theory]
        [InlineData("", null)]
        [InlineData(".", null)]
        [InlineData(":", null)]
        [InlineData("'", null)]
        [InlineData(" ", null)]
        [InlineData("   ", null)]
        [InlineData("127.0.0.1", null)]
        [InlineData("0.0.0.0", null)]
        [InlineData("example.com", "example.com")]
        [InlineData("javascript:alert('x')", null)]
        public void CreateSafeUri_ValidatesHost(string input, string? expected)
        {
            var result = Utils.CreateSafeUri(input);
            Assert.Equal(expected, result?.Host);
        }


        [Theory]
        [InlineData("example.com", "https://example.com/")]
        [InlineData("http://example.com", "http://example.com/")]
        [InlineData("https://example.com", "https://example.com/")]
        [InlineData("ftp://example.com", null)]
        public void CreateSafeUri_UsesHttpsUnlessInputUsesHttp(string input, string? expected)
        {
            var result = Utils.CreateSafeUri(input);
            Assert.Equal(expected, result?.ToString());
        }
    }
}
