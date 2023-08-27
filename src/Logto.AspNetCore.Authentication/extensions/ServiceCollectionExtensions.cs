using System;
using Microsoft.Extensions.DependencyInjection;

namespace Logto.AspNetCore.Authentication;

/// <summary>
/// The Logto extension methods for ServiceCollectionExtensions.
/// </summary>
public static class ServiceCollectionExtensions
{
  /// <summary>
  /// Add Logto authentication services to the specified <see cref="IServiceCollection" />.
  /// </summary>
  /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
  /// <param name="configureOptions">A delegate to configure <see cref="LogtoOptions"/>.</param>
  /// <returns>A reference to this instance after the operation has completed.</returns>
  public static IServiceCollection AddLogtoAuthentication(this IServiceCollection services, Action<LogtoOptions> configureOptions)
  {
    services.AddLogtoAuthentication(LogtoDefaults.AuthenticationScheme, configureOptions);

    return services;
  }

  /// <summary>
  /// Add Logto authentication services to the specified <see cref="IServiceCollection" />.
  /// </summary>
  /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
  /// <param name="authenticationScheme">The authentication scheme.</param>
  /// <param name="configureOptions">A delegate to configure <see cref="LogtoOptions"/>.</param>
  /// <returns>A reference to this instance after the operation has completed.</returns>
  public static IServiceCollection AddLogtoAuthentication(this IServiceCollection services, string authenticationScheme, Action<LogtoOptions> configureOptions)
  {
    services.AddLogtoAuthentication(authenticationScheme, LogtoDefaults.CookieScheme, configureOptions);

    return services;
  }

  /// <summary>
  /// Add Logto authentication services to the specified <see cref="IServiceCollection" />.
  /// </summary>
  /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
  /// <param name="authenticationScheme">The authentication scheme.</param>
  /// <param name="cookieScheme">The cookie scheme.</param>
  /// <param name="configureOptions">A delegate to configure <see cref="LogtoOptions"/>.</param>
  /// <returns>A reference to this instance after the operation has completed.</returns>
  public static IServiceCollection AddLogtoAuthentication(this IServiceCollection services, string authenticationScheme, string cookieScheme, Action<LogtoOptions> configureOptions)
  {
    services
      .AddAuthentication(options =>
      {
        options.DefaultScheme = cookieScheme;
        options.DefaultChallengeScheme = authenticationScheme;
        options.DefaultSignOutScheme = authenticationScheme;
      })
      .AddLogto(authenticationScheme, cookieScheme, configureOptions);

    return services;
  }
}
