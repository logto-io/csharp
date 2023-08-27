using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Logto.AspNetCore.Authentication;

public static class HttpContextExtensions
{
  /// <summary>
  /// Get the Logto options from the <see cref="HttpContext"/>.
  /// </summary>
  /// <param name="httpContext">The <see cref="HttpContext"/>.</param>
  /// <returns>The <see cref="LogtoOptions"/>.</returns>
  public static LogtoOptions GetLogtoOptions(this HttpContext httpContext)
  {
    return httpContext.GetLogtoOptions(LogtoDefaults.AuthenticationScheme);
  }

  /// <summary>
  /// Get the Logto options from the <see cref="HttpContext"/>.
  /// </summary>
  /// <param name="httpContext">The <see cref="HttpContext"/>.</param>
  /// <param name="authenticationScheme">The authentication scheme.</param>
  /// <returns>The <see cref="LogtoOptions"/>.</returns>
  public static LogtoOptions GetLogtoOptions(this HttpContext httpContext, string authenticationScheme)
  {
    var options = httpContext.RequestServices.GetRequiredService<IOptionsMonitor<LogtoOptions>>().Get(authenticationScheme);
    if (options is null)
    {
      throw new InvalidOperationException($"No authentication scheme configured for {authenticationScheme}.");
    }

    return options;
  }
}
