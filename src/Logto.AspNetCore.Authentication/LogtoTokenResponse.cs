using System.Text.Json.Serialization;

namespace Logto.AspNetCore.Authentication;

/// <summary>
/// The <a href="https://openid.net/specs/openid-connect-core-1_0.html#TokenResponse">successful token response</a> from Logto authorization server.
/// </summary>
public class LogtoTokenResponse
{
  /// <summary>
  /// The access token issued by the Logto authorization server.
  /// </summary>
  [JsonPropertyName("access_token")]
  public string AccessToken { get; set; } = null!;

  /// <summary>
  /// The type of the token issued by the Logto authorization server.
  /// </summary>
  [JsonPropertyName("token_type")]
  public string TokenType { get; set; } = null!;

  /// <summary>
  /// The lifetime in seconds of the access token.
  /// </summary>
  [JsonPropertyName("expires_in")]
  public int ExpiresIn { get; set; }

  /// <summary>
  /// The refresh token, which can be used to obtain new access tokens using the same authorization grant.
  /// </summary>
  [JsonPropertyName("refresh_token")]
  public string? RefreshToken { get; set; } = null!;

  /// <summary>
  /// The ID token, which can be used to verify the identity of the user.
  /// </summary>
  [JsonPropertyName("id_token")]
  public string? IdToken { get; set; } = null;
}
