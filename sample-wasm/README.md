# Logto ASP.NET Blazor WebAssembly sample project

This sample project shows how to use the [Blorc.OpenIdConnect](https://github.com/WildGums/Blorc.OpenIdConnect) to authenticate users with Logto in a Blazor WebAssembly application.

## Prerequisites

- .NET 6.0 or higher
- A [Logto Cloud](https://logto.io/) account or a self-hosted Logto
- A Logto single-page application created

### Optional

- Set up an API resource in Logto

If you don't have the Logto application created, please follow the [⚡ Get started](https://docs.logto.io/docs/tutorials/get-started/) guide to create one.

## Configuration

Create an `appsettings.Development.json` (or `appsettings.json`) with the following structure:

```jsonc
{
  // ...
  "IdentityServer": {
    "Authority": "https://<your-logto-endpoint>/oidc",
    "ClientId": "<your-logto-app-id>",
    "PostLogoutRedirectUri": "<your-app-url>", // Remember to configure this in Logto
    "RedirectUri": "<your-app-url>", // Remember to configure this in Logto
    "ResponseType": "code",
    "Scope": "openid profile" // Add more scopes if needed
  }
}
```

### Fetch user info

For some special claims, such as `custom_data`, calling the `/userinfo` endpoint is required. To enable this feature, add the following configuration:

```jsonc
{
  // ...
  "IdentityServer": {
    // ...
    "LoadUserInfo": true
  }
}
```

> [!Caution]
> Since WebAssembly is a client-side application, the token request will only be sent to the server-side once. Due to this nature, `LoadUserInfo` is conflict with fetching access token for API resources.

### JWT access token

If you need to fetch an access token in JWT format for an API resource, add the following configuration:

```jsonc
{
  // ...
  "IdentityServer": {
    // ...
    "Resource": "https://<your-api-resource-indicator>",
    "ExtraTokenParams": {
      "resource": "https://<your-api-resource-indicator>" // Ensure the key is lowercase
    }
  }
}
```

The value of `Resource` and `ExtraTokenParams.resource` should be the same.

## Run the sample

```bash
dotnet run # or `dotnet watch` to run in watch mode
```
