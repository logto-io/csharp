namespace Logto.AspNetCore.Authentication.Tests;

public class LogtoCookieContextManagerTests
{
    [Theory]
    [InlineData("https://www.example.com/", "https://www.example.com/oidc/token")]
    [InlineData("https://www.example.com", "https://www.example.com/oidc/token")]
    [InlineData("https://example.com", "https://example.com/oidc/token")]
    [InlineData("http://www.example.com", "http://www.example.com/oidc/token")]
    [InlineData("https://sub.example.com", "https://sub.example.com/oidc/token")]
    [InlineData("https://www.example.com/path/", "https://www.example.com/path/oidc/token")]
    public void FetchTokenUriParseTest(string endpoint, string expectedUri)
    {
        var requestUri = LogtoCookieContextManager.GetOidcTokenRequestUri(endpoint);
        Assert.Equal(requestUri.ToString(), expectedUri);
    }

    [Fact]
    public void FetchTokenUriInvalidFormatTest()
    {
        Assert.Throws<UriFormatException>(() => LogtoCookieContextManager.GetOidcTokenRequestUri("https:///example.com//"));
    }

    [Fact]
    public void FetchTokenUriNullOrEmptyTest()
    {
        Assert.Throws<ArgumentNullException>(() => LogtoCookieContextManager.GetOidcTokenRequestUri(null!));
        Assert.Throws<UriFormatException>(() => LogtoCookieContextManager.GetOidcTokenRequestUri(string.Empty));
    }

}