namespace Logto.AspNetCore.Authentication.Tests;

public class LogtoUtilsTests_GetExpiresAt
{
    [Fact]
    public void ShouldReturnFutureDate()
    {
        // Add 1 second to the current time, should be in the future since tests are fast
        var expiresAtString = LogtoUtils.GetExpiresAt(1);
        var expiresAt = DateTimeOffset.Parse(expiresAtString);
        var now = DateTimeOffset.UtcNow;
        Assert.True(expiresAt > now);
    }
}

public class LogtoUtilsTests_IsExpired
{
    [Fact]
    public void ShouldReturnTrueIfTimeStringIsNullOrEmpty()
    {
        Assert.True(LogtoUtils.IsExpired(null));
        Assert.True(LogtoUtils.IsExpired(string.Empty));
    }

    [Fact]
    public void ShouldReturnTrueIfTimeStringIsNotParsable()
    {
        Assert.True(LogtoUtils.IsExpired("foo"));
    }

    [Fact]
    public void ShouldReturnTrueIfTimeStringIsExpired()
    {
        var expiresAt = DateTimeOffset.UtcNow.AddSeconds(-1);
        Assert.True(LogtoUtils.IsExpired(expiresAt.ToString("o")));
    }

    [Fact]
    public void ShouldReturnFalseIfTimeStringIsNotExpired()
    {
        var expiresAt = DateTimeOffset.UtcNow.AddSeconds(1);
        Assert.False(LogtoUtils.IsExpired(expiresAt.ToString("o")));
    }
}
