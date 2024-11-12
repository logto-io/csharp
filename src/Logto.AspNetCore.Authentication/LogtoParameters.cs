using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Collections.Generic;

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
  }

  /// <summary>
  /// The authentication parameters for Logto sign-in experience customization.
  /// </summary>
  public static class Authentication
  {
    /// <summary>
    /// The first screen to show in the sign-in experience.
    /// </summary>
    public static class FirstScreen
    {
      /// <summary>
      /// Show the register form first.
      /// </summary>
      public const string Register = "identifier:register";

      /// <summary>
      /// Show the sign-in form first.
      /// </summary>
      public const string SignIn = "identifier:sign_in";

      /// <summary>
      /// Show the single sign-on form first.
      /// </summary>
      public const string SingleSignOn = "single_sign_on";

      /// <summary>
      /// Show the reset password form first.
      /// </summary>
      public const string ResetPassword = "reset_password";
    }

    /// <summary>
    /// The identifiers to use for authentication.
    /// This parameter MUST be used together with <see cref="FirstScreen"/>.
    /// </summary>
    public static class Identifiers
    {
      /// <summary>
      /// Use email for authentication.
      /// </summary>
      public const string Email = "email";
      
      /// <summary>
      /// Use phone for authentication.
      /// </summary>
      public const string Phone = "phone";
      
      /// <summary>
      /// Use username for authentication.
      /// </summary>
      public const string Username = "username";
    }

    /// <summary>
    /// Direct sign-in configuration.
    /// </summary>
    public class DirectSignIn
    {
      /// <summary>
      /// The target identifier for direct sign-in.
      /// </summary>
      public string Target { get; set; } = string.Empty;

      /// <summary>
      /// The sign-in method.
      /// </summary>
      public string Method { get; set; } = string.Empty;

      public static class Methods
      {
        /// <summary>
        /// Single sign-on method.
        /// </summary>
        public const string Sso = "sso";

        /// <summary>
        /// Social sign-in method.
        /// </summary>
        public const string Social = "social";
      }
    }

    /// <summary>
    /// Extra parameters to be passed to the authorization endpoint.
    /// </summary>
    public class ExtraParams : Dictionary<string, string>
    {
    }
  }
}
