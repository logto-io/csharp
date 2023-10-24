using System.Collections.Generic;

namespace Logto.AspNetCore.Authentication;

/// <summary>
/// Options for configuring Logto authentication.
/// </summary>
public class LogtoOptions
{
  /// <summary>
  /// The endpoint for the Logto server, you can get it from the integration guide or the application details page.
  /// </summary>
  /// <example>
  /// https://foo.app.logto.dev/
  /// </example>
  public string Endpoint { get; set; } = null!;
  /// <summary>
  /// The client ID of your application, you can get it from the integration guide or the application details page.
  /// </summary>
  public string AppId { get; set; } = null!;
  /// <summary>
  /// The client secret of your application, you can get it from the integration guide or the application details page.
  /// </summary>
  public string? AppSecret { get; set; } = null;
  /// <summary>
  /// The scopes (permissions) that your application needs to access.
  /// Scopes that will be added by default: `openid`, `offline_access` and `profile`.
  /// <br/>
  /// If resource is specified, scopes will be applied to it when requesting access tokens for the resource.
  /// </summary>
  public ICollection<string> Scopes { get; set; } = new List<string>();
  /// <summary>
  /// The API resource that your application needs to access.
  /// See <a href="https://docs.logto.io/docs/recipes/rbac/">RBAC</a> to learn more about how to use role-based access control (RBAC) to protect API resources.
  /// </summary>
  public string? Resource { get; set; } = null;
  /// <summary>
  /// The prompt parameter for the OpenID Connect authorization request.
  /// <br/>
  /// - If the value is `consent`, the user will be able to reuse the existing consent without being prompted for sign-in again.
  /// <br/>
  /// - If the value is `login`, the user will be prompted for sign-in again anyway. Note there will be no refresh token returned in this case.
  /// </summary>
  public string Prompt { get; set; } = PromptMode.Consent;
  /// <summary>
  /// The callback path for the OpenID Connect authorization response. (default: "/Callback")
  /// <br/>
  /// The full URI after appending the callback path will be used as the redirect URI for the OpenID Connect authorization request.
  /// <br/>
  /// It should be the same as the one you configured in the Logto Console.
  /// </summary>
  public string CallbackPath { get; set; } = "/Callback";
  /// <summary>
  /// The callback path for the OpenID Connect sign-out response. (default: "/SignedOutCallback")
  /// <br/>
  /// The full URI after appending the callback path will be used as the redirect URI for the OpenID Connect sign-out request.
  /// </summary>
  public string SignedOutCallbackPath { get; set; } = "/SignedOutCallback";
  /// <summary>
  /// Boolean to set whether get claims from OpenID Connect userinfo endpoint. (default: false)
  /// <br/>
  /// For some claims that may be in a large size, such as `custom_data` and `identities`, it is required to
  /// set this value to `true` since they are not included in the ID token.
  /// </summary>
  public bool GetClaimsFromUserInfoEndpoint { get; set; } = false;
  /// <summary>
  /// The domain to associate the cookie with. Allows multiple applications to share the cookie such as on sub-domains. 
  /// </summary>
  public string? CookieDomain { get; set; } = null;
}

/// <summary>
/// The prompt parameter for the OpenID Connect authorization request.
/// </summary>
public static class PromptMode
{
  /// <summary>
  /// The user will be able to reuse the existing consent without being prompted for sign-in again.
  /// </summary>
  public const string Consent = "consent";
  /// <summary>
  /// The user will be prompted for sign-in again anyway. Note there will be no refresh token returned in this case.
  /// </summary>
  public const string Login = "login";
}
