namespace Logto.AspNetCore.Authentication.Tests;

public class LogtoCookieContextManagerTests
{
    [Theory]
    [InlineData("https://www.example.com/", "https://www.example.com/oidc/token")]
    [InlineData("https://www.example.com", "https://www.example.com/oidc/token")]
    public void FetchTokenUriParseTest(string endpoint, string expectedUri)
    {
        var requestUri = LogtoCookieContextManager.GetTokenRequestUri(endpoint);
        Assert.Equal(requestUri.ToString(), expectedUri);
    }
}