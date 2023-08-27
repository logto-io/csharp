using System;

namespace Logto.AspNetCore.Authentication;

public static class LogtoUtils
{
  /// <summary>
  /// Given a time string, return if it is expired compared to the current time.
  /// If the time string is null, empty, or it cannot be parsed, return true.
  /// </summary>
  /// <param name="timeString">The time string to parse.</param>
  /// <returns>True if the time string is expired, false otherwise.</returns>
  public static bool IsExpired(string? timeString)
  {
    if (string.IsNullOrEmpty(timeString))
    {
      return true;
    }

    // Check is the time string is an epoch time
    if (long.TryParse(timeString, out var epochTime))
    {
      var epochTimeOffset = DateTimeOffset.FromUnixTimeSeconds(epochTime);
      return epochTimeOffset < DateTimeOffset.UtcNow;
    }

    // Check if the time string is an ISO 8601 time
    if (!DateTimeOffset.TryParse(timeString, out var time))
    {
      return true;
    }

    return time < DateTimeOffset.UtcNow;
  }

  /// <summary>
  /// Given a time in seconds, return a time string that is that many seconds in the future.
  /// </summary>
  /// <param name="expiresIn">The number of seconds in the future.</param>
  /// <returns>A time string that is that many seconds in the future.</returns>
  public static string GetExpiresAt(int expiresIn)
  {
    return DateTimeOffset.UtcNow.AddSeconds(expiresIn).ToString("o");
  }
}
