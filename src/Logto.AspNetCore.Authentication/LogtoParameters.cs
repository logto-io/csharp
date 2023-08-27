using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Logto.AspNetCore.Authentication;

public static class LogtoParameters
{
  /// <summary>
  /// The token names used by Cookie and OpenID Connect middleware to store and retrieve tokens from
  /// Logto OpenID Connect provider.
  /// <br/>
  /// See <see href="https://github.com/dotnet/aspnetcore/blob/4a9118c674a798aaf6379b4b7b2d8787bc688f96/src/Security/Authentication/OpenIdConnect/src/OpenIdConnectHandler.cs#L994-L1035">tokens that are stored by OpenID Connect middleware</see> for more details.
  /// </summary>
  public static class Tokens
  {
    public const string AccessToken = OpenIdConnectParameterNames.AccessToken;
    public const string ExpiresAt = "expires_at";
    public const string AccessTokenForResource = $"{AccessToken}.resource";
    public const string ExpiresAtForResource = $"{ExpiresAt}.resource";
    public const string RefreshToken = OpenIdConnectParameterNames.RefreshToken;
    public const string IdToken = OpenIdConnectParameterNames.IdToken;
    public const string TokenType = OpenIdConnectParameterNames.TokenType;

  }

  /// <summary>
  /// The scope names used by Logto OpenID Connect provider to request for user information.
  /// </summary>
  public static class Scopes
  {
    /// <summary>
    /// The scope name for requesting user's email.
    /// Logto will issue two claims to the ID token: <c>email</c> and <c>email_verified</c>.
    /// </summary>
    public const string Email = "email";
    /// <summary>
    /// The scope name for requesting user's phone number.
    /// Logto will issue two claims to the ID token: <c>phone</c> and <c>phone_verified</c>.
    /// </summary>
    public const string Phone = "phone";
    /// <summary>
    /// The scope name for requesting user's custom data.
    /// Logto will issue a claim to the response of the <c>userinfo</c> endpoint: <c>custom_data</c>.
    /// <br/>
    /// Note that when requesting this scope, you must set <see cref="LogtoOptions.GetClaimsFromUserInfoEndpoint"/> to <c>true</c>.
    /// </summary>
    public const string CustomData = "custom_data";
    /// <summary>
    /// The scope name for requesting user's identities.
    /// Logto will issue a claim to the response of the <c>userinfo</c> endpoint: <c>identities</c>.
    /// <br/>
    /// Note that when requesting this scope, you must set <see cref="LogtoOptions.GetClaimsFromUserInfoEndpoint"/> to <c>true</c>.
    /// </summary>
    public const string Identities = "identities";
  }
}
