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
    /// <summary>
    /// The scope for user's organization IDs and perform organization token grant per <see href="https://github.com/logto-io/rfcs">RFC 0001</see>.
    /// <br/>
    /// To learn more about Logto Organizations, see <see href="https://docs.logto.io/docs/recipes/organizations/" />.
    /// </summary>
    public const string Organizations = "urn:logto:scope:organizations";
    /// <summary>
    /// Scope for user's organization roles per <see href="https://github.com/logto-io/rfcs">RFC 0001</see>.
    /// <br/>
    /// To learn more about Logto Organizations, see <see href="https://docs.logto.io/docs/recipes/organizations/" />.
    /// </summary>
    public const string OrganizationRoles = "urn:logto:scope:organization_roles";
  }

  /// <summary>
  /// The claim names used by Logto OpenID Connect provider for ID token and userinfo endpoint.
  /// </summary>
  public static class Claims
  {
    /// <summary>
    /// The claim name for the issuer identifier for whom issued the token.
    /// </summary>
    public const string Issuer = "iss";
    /// <summary>
    /// The claim name for the subject identifier for whom the token is intended (user ID).
    /// </summary>
    public const string Subject = "sub";
    /// <summary>
    /// The claim name for the audience that the token is intended for, which is the client ID.
    /// </summary>
    public const string Audience = "aud";
    /// <summary>
    /// The claim name for the expiration time of the token (in seconds).
    /// </summary>
    public const string Expiration = "exp";
    /// <summary>
    /// The claim name for the time at which the token was issued (in seconds).
    /// </summary>
    public const string IssuedAt = "iat";
    /// <summary>
    /// The claim name for the user's full name.
    /// </summary>
    public const string Name = "name";
    /// <summary>
    /// The claim name for user's username.
    /// </summary>
    public const string Username = "username";
    /// <summary>
    /// The claim name for user's profile picture URL.
    /// </summary>
    public const string Picture = "picture";
    /// <summary>
    /// The claim name for user's email.
    /// </summary>
    public const string Email = "email";
    /// <summary>
    /// The claim name for user's email verification status.
    /// </summary>
    public const string EmailVerified = "email_verified";
    /// <summary>
    /// The claim name for user's phone number.
    /// </summary>
    public const string PhoneNumber = "phone_number";
    /// <summary>
    /// The claim name for user's phone number verification status.
    /// </summary>
    public const string PhoneNumberVerified = "phone_number_verified";
    /// <summary>
    /// The claim name for user's custom data.
    /// </summary>
    public const string CustomData = "custom_data";
    /// <summary>
    /// The claim name for user's identities.
    /// </summary>
    public const string Identities = "identities";
    /// <summary>
    /// The claim name for user's organization IDs.
    /// </summary>
    public const string Organizations = "organizations";
    /// <summary>
    /// The claim name for user's organization roles. Each role is in the format of `<organization_id>:<role_name>`.
    /// </summary>
    public const string OrganizationRoles = "organization_roles";
  }

  /// <summary>
  /// Resources that reserved by Logto, which cannot be defined by users.
  /// </summary>
  public static class ReservedResource
  {
    /// <summary>
    /// The resource for organization template per <see href="https://github.com/logto-io/rfcs">RFC 0001</see>.
    /// </summary>
    public const string Organizations = "urn:logto:resource:organizations";
  }
}
