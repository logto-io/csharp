namespace Logto.AspNetCore.Authentication;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;

/// <summary>
/// Extension methods to configure Logto authentication.
/// </summary>
/// <remarks>
/// <para>
/// Logto is an identity layer on top of the Open ID Connect protocol. This extension leverages the Open ID Connect
/// authentication handler but adds additional functionality to support Logto's requirements.
/// </para>
/// </remarks>
public static class AuthenticationBuilderExtensions
{
  /// <summary>
  /// Adds Logto authentication to <see cref="AuthenticationBuilder"/> using the default scheme.
  /// The default scheme is specified by <see cref="LogtoDefaults.AuthenticationScheme"/>, which is "Logto";
  /// The default cookie scheme is specified by <see cref="LogtoDefaults.CookieScheme"/>, which is "Logto.Cookie";
  /// <param name="builder">The <see cref="AuthenticationBuilder"/>.</param>
  /// <param name="configureOptions">A delegate to configure <see cref="LogtoOptions"/>.</param>
  /// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
  public static AuthenticationBuilder AddLogto(this AuthenticationBuilder builder, Action<LogtoOptions> configureOptions)
      => builder.AddLogto(LogtoDefaults.AuthenticationScheme, configureOptions);


  /// <summary>
  /// Adds Logto authentication to <see cref="AuthenticationBuilder"/> using the specified scheme.
  /// The default cookie scheme is specified by <see cref="LogtoDefaults.CookieScheme"/>, which is "Logto.Cookie";
  /// </summary>
  /// <param name="builder">The <see cref="AuthenticationBuilder"/>.</param>
  /// <param name="authenticationScheme">The authentication scheme.</param>
  /// <param name="configureOptions">A delegate to configure <see cref="LogtoOptions"/>.</param>
  /// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
  public static AuthenticationBuilder AddLogto(this AuthenticationBuilder builder, string authenticationScheme, Action<LogtoOptions> configureOptions)
      => builder.AddLogto(authenticationScheme, LogtoDefaults.CookieScheme, configureOptions);

  /// <summary>
  /// Adds Logto authentication to <see cref="AuthenticationBuilder"/> using the specified scheme and cookie scheme.
  /// </summary>
  /// <param name="builder">The <see cref="AuthenticationBuilder"/>.</param>
  /// <param name="authenticationScheme">The authentication scheme.</param>
  /// <param name="cookieScheme">The cookie scheme.</param>
  /// <param name="configureOptions">A delegate to configure <see cref="LogtoOptions"/>.</param>
  /// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
  public static AuthenticationBuilder AddLogto(this AuthenticationBuilder builder, string authenticationScheme, string cookieScheme, Action<LogtoOptions> configureOptions)
  {
    var logtoOptions = new LogtoOptions();
    configureOptions(logtoOptions);

    builder.Services.Configure(authenticationScheme, configureOptions);
    builder.Services
      .AddOptions<CookieAuthenticationOptions>(cookieScheme)
      .Configure((options) => ConfigureCookieOptions(authenticationScheme, options, logtoOptions));
    builder.AddCookie(cookieScheme);
    builder.AddOpenIdConnect(authenticationScheme, oidcOptions => ConfigureOpenIdConnectOptions(oidcOptions, logtoOptions, cookieScheme));

    return builder;
  }

  /// <summary>
  /// Configures the cookie options for Logto authentication. This method will mutate the `options` parameter.
  /// </summary>
  private static void ConfigureCookieOptions(string authenticationScheme, CookieAuthenticationOptions options, LogtoOptions logtoOptions)
  {
    options.Cookie.Name = $"Logto.Cookie.{logtoOptions.AppId}";
    options.SlidingExpiration = true;
    options.Cookie.Domain = logtoOptions.CookieDomain;
    options.Events = new CookieAuthenticationEvents
    {
      OnValidatePrincipal = context => new LogtoCookieContextManager(authenticationScheme, context).Handle()
    };
  }

  /// <summary>
  /// Configures the OpenID Connect options for Logto authentication. This method will mutate the `options` parameter.
  /// </summary>
  /// <param name="options">The OpenID Connect options to configure.</param>
  /// <param name="logtoOptions">The Logto options to use for configuration.</param>
  private static void ConfigureOpenIdConnectOptions(OpenIdConnectOptions options, LogtoOptions logtoOptions, string cookieScheme)
  {
    options.Authority = logtoOptions.Endpoint + "oidc";
    options.ClientId = logtoOptions.AppId;
    options.ClientSecret = logtoOptions.AppSecret;
    options.ResponseType = OpenIdConnectResponseType.Code;
    options.SaveTokens = true;
    options.UsePkce = true;
    options.Prompt = logtoOptions.Prompt;
    options.CallbackPath = new PathString(logtoOptions.CallbackPath);
    options.SignedOutCallbackPath = new PathString(logtoOptions.SignedOutCallbackPath);
    options.GetClaimsFromUserInfoEndpoint = logtoOptions.GetClaimsFromUserInfoEndpoint;
    options.MapInboundClaims = false;
    options.ClaimActions.MapAllExcept("nbf", "nonce", "c_hash", "at_hash");
    options.Events = new OpenIdConnectEvents
    {
      OnRedirectToIdentityProviderForSignOut = async context =>
      {
        // Clean up the cookie when signing out.
        await context.HttpContext.SignOutAsync(cookieScheme);

        // Rebuild parameters since we use <c>client_id</c> for sign-out, no need to use <c>id_token_hint</c>.
        context.ProtocolMessage.Parameters.Remove(OpenIdConnectParameterNames.IdTokenHint);
        context.ProtocolMessage.Parameters.Add(OpenIdConnectParameterNames.ClientId, logtoOptions.AppId);
      },
    };
    options.TokenValidationParameters = new TokenValidationParameters
    {
      NameClaimType = "name",
      RoleClaimType = "role",
      ValidateAudience = true,
      ValidAudience = logtoOptions.AppId,
      ValidateIssuer = true,
      ValidIssuer = logtoOptions.Endpoint + "oidc",
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,
    };

    // Handle scopes
    var scopes = new HashSet<string>(logtoOptions.Scopes)
      {
          "openid",
          "offline_access",
          "profile"
      };

    options.Scope.Clear();
    foreach (var scope in scopes)
    {
      options.Scope.Add(scope);
    }

    // Handle resource
    if (!string.IsNullOrEmpty(logtoOptions.Resource))
    {
      options.Resource = logtoOptions.Resource;
    }
  }
}
