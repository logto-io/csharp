using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Logto.AspNetCore.Authentication;

/// <summary>
/// Manage CookieValidatePrincipalContext for Logto authentication.
/// It is used to check if the tokens in cookie is expired or not, and refresh using refresh token if needed.
/// </summary>
public class LogtoCookieContextManager
{
  private readonly string authenticationScheme;
  private readonly CookieValidatePrincipalContext context;
  private LogtoOptions logtoOptions
  {
    get => context.HttpContext.RequestServices.GetRequiredService<IOptionsMonitor<LogtoOptions>>().Get(authenticationScheme);
  }
  private OpenIdConnectOptions oidcOptions
  {
    get => context.HttpContext.RequestServices.GetRequiredService<IOptionsMonitor<OpenIdConnectOptions>>().Get(authenticationScheme);
  }
  private HttpClient httpClient
  {
    get => oidcOptions.Backchannel;
  }

  public LogtoCookieContextManager(
    string authenticationScheme,
    CookieValidatePrincipalContext context
  )
    => (
      this.authenticationScheme,
      this.context
    ) = (
      authenticationScheme,
      context
    );

  /// <summary>
  /// Check if the cookie is expired or not, and refresh using refresh token if needed.
  /// </summary>
  public async Task Handle()
  {
    Debug.WriteLine("Validating principal...");

    // Reject the principal if the authentication scheme has been explicitly set to something other than
    // the scheme configured for Logto authentication.
    if (context.Properties.Items.TryGetValue(".AuthScheme", out var authScheme))
    {
      if (authScheme != authenticationScheme)
      {
        context.RejectPrincipal();
        return;
      }
    }

    // Check if access token is expired.
    // We don't check for the ID token because this hard to update claims since the <a href="https://github.com/dotnet/aspnetcore/blob/4a9118c674a798aaf6379b4b7b2d8787bc688f96/src/Security/Authentication/OpenIdConnect/src/OpenIdConnectHandler.cs#L795-L809">
    // original logic</a> in OpenIdConnectHandler.cs is complicated not exposed and we don't want to copy it here.
    // Until we implement a full-fledged OIDC middleware for Logto, we just check for the access token.

    if (!await RefreshTokens())
    {
      context.RejectPrincipal();
      return;
    }

    if (logtoOptions.Resource != null && !await RefreshTokens(true))
    {
      context.RejectPrincipal();
    }
  }

  /// <summary>
  /// Check if the access token for the resource is expired or not, and refresh using refresh token if needed.
  /// </summary>
  /// <param name="forResource">If it's for the resource specified in the LogtoOptions.</param>
  /// <returns>True if the access token is not expired, or it is expired but refreshed successfully. False otherwise.</returns>
  public async Task<bool> RefreshTokens(bool forResource = false)
  {
    var expiredAtKey = forResource ? LogtoParameters.Tokens.ExpiresAtForResource : LogtoParameters.Tokens.ExpiresAt;
    var accessTokenKey = forResource ? LogtoParameters.Tokens.AccessTokenForResource : LogtoParameters.Tokens.AccessToken;

    // Use a dictionary to store the tokens so that we can update them without checking the duplicated keys.
    Dictionary<string, string> tokenSet = context.Properties.GetTokens().ToDictionary(t => t.Name, t => t.Value);

    if (
      !LogtoUtils.IsExpired(tokenSet.GetValueOrDefault(expiredAtKey))
    )
    {
      Debug.WriteLine("Token not expired. No need to refresh.");
      return true;
    }

    Debug.WriteLine("Token expired. Refreshing tokens...");

    var refreshToken = tokenSet.GetValueOrDefault(LogtoParameters.Tokens.RefreshToken);

    if (string.IsNullOrEmpty(refreshToken))
    {
      return false;
    }

    LogtoTokenResponse? tokens;

    try
    {
      tokens = await FetchTokensByRefreshToken(refreshToken, forResource);
    }
    catch (Exception exception)
    {
      Debug.WriteLine("Failed to fetch tokens by refresh token.");
      Debug.WriteLine(exception);
      return false;
    }

    tokenSet[accessTokenKey] = tokens.AccessToken;
    tokenSet[expiredAtKey] = LogtoUtils.GetExpiresAt(tokens.ExpiresIn);

    if (!string.IsNullOrEmpty(tokens.RefreshToken))
    {
      tokenSet[LogtoParameters.Tokens.RefreshToken] = tokens.RefreshToken;
    }

    if (!string.IsNullOrEmpty(tokens.IdToken))
    {
      tokenSet[LogtoParameters.Tokens.IdToken] = tokens.IdToken;
    }

    // Write the updated tokens back to the cookie.
    context.Properties.StoreTokens(tokenSet.Select(t => new AuthenticationToken
    {
      Name = t.Key,
      Value = t.Value
    }));
    context.ShouldRenew = true;
    return true;

  }

  /// <summary>
  /// Fetch tokens by making a request to the Logto token endpoint with a refresh token.
  /// </summary>
  /// <param name="refreshToken">The refresh token to use to fetch new tokens.</param>
  /// <param name="forResource">If it's for the resource specified in the LogtoOptions.</param>
  /// <returns>The response from the token endpoint.</returns>
  /// <exception cref="HttpRequestException">Thrown if the request to the token endpoint fails.</exception>
  /// <exception cref="JsonException">Thrown if the response from the token endpoint cannot be parsed.</exception>
  public async Task<LogtoTokenResponse> FetchTokensByRefreshToken(string refreshToken, bool forResource)
  {
    var body = new Dictionary<string, string>
    {
      ["grant_type"] = "refresh_token",
      ["client_id"] = logtoOptions.AppId,
      ["refresh_token"] = refreshToken,
    };

    if (!string.IsNullOrEmpty(logtoOptions.AppSecret))
    {
      body["client_secret"] = logtoOptions.AppSecret;
    }

    if (forResource)
    {
      body["resource"] = logtoOptions.Resource!;
    }

    // TODO: The token endpoint should be read from the discovery endpoint or the OpenID Connect context.
    var request = new HttpRequestMessage(HttpMethod.Post, GetOidcTokenRequestUri(logtoOptions.Endpoint))
    {
      Content = new FormUrlEncodedContent(body)
    };

    var response = await httpClient.SendAsync(request);
    response.EnsureSuccessStatusCode();

    var responseString = await response.Content.ReadAsStringAsync();
    return JsonSerializer.Deserialize<LogtoTokenResponse>(responseString, new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true,
    })!;
  }

  /// <summary>
  /// Constructs a URI for the OpenID Connect (OIDC) token request based on the provided endpoint.
  /// </summary>
  /// <param name="endpoint">The base endpoint URL as a string.</param>
  /// <returns>A <see cref="Uri"/> object representing the full token request URI.</returns>
  /// <exception cref="UriFormatException">Thrown when the provided endpoint is not a valid URI.</exception>
  public static Uri GetOidcTokenRequestUri(string endpoint)
  {
    var baseUri = new Uri(endpoint);
    var requestUri = new Uri(baseUri, "oidc/token");
    return requestUri;
  }
}
