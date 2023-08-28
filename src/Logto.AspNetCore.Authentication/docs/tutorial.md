# Logto ASP.NET Core authentication tutorial

This tutorial will show you how to use Logto ASP.NET Core authentication middleware to protect your web application.

## Table of contents

- [Logto ASP.NET Core authentication tutorial](#logto-aspnet-core-authentication-tutorial)
  - [Table of contents](#table-of-contents)
  - [Integration](#integration)
    - [Add Logto authentication middleware](#add-logto-authentication-middleware)
    - [Sign-in](#sign-in)
    - [Sign-out](#sign-out)
    - [Implement sign-in/sign-out buttons (Razor Pages)](#implement-sign-insign-out-buttons-razor-pages)
    - [Checkpoint: Run the web application](#checkpoint-run-the-web-application)
  - [The user object](#the-user-object)
    - [Some claims are missing](#some-claims-are-missing)
  - [Scopes and claims](#scopes-and-claims)
    - [Special ID token claims](#special-id-token-claims)
  - [API resources](#api-resources)
    - [Configure Logto client](#configure-logto-client)
  - [Fetch tokens](#fetch-tokens)

## Integration

### Add Logto authentication middleware

Open `Startup.cs` (or `Program.cs`) and add the following code to register Logto authentication middleware:

```csharp
using Logto.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogtoAuthentication(options =>
{
  options.Endpoint = builder.Configuration["Logto:Endpoint"]!;
  options.AppId = builder.Configuration["Logto:AppId"]!;
  options.AppSecret = builder.Configuration["Logto:AppSecret"];
});

app.UseAuthentication();
```

The `AddLogtoAuthentication` method will do the following things:

- Set the default authentication scheme to `LogtoDefaults.CookieScheme`.
- Set the default challenge scheme to `LogtoDefaults.AuthenticationScheme`.
- Set the default sign-out scheme to `LogtoDefaults.AuthenticationScheme`.
- Add cookie and OpenID Connect authentication handlers to the authentication scheme.

### Sign-in

To sign-in with Logto, you can use the `ChallengeAsync` method:

```csharp
await HttpContext.ChallengeAsync(new AuthenticationProperties
{
  RedirectUri = "/"
});
```

The `ChallengeAsync` method will redirect the user to the Logto sign-in page.

The `RedirectUri` property is used to redirect the user back to your web application after authentication. Note it is different from the redirect URI you configured in the Logto application details page:

1. The redirect URI in the Logto application details page is the URI that Logto will redirect the user back to after the user has signed in.
2. The `RedirectUri` property is the URI that will be redirected to after necessary actions have been taken in the Logto authentication middleware.

The order of the actions is 1 -> 2. For clarity, let's call the redirect URI in the Logto application details page the **Logto redirect URI** and the `RedirectUri` property the **application redirect URI**.

The **Logto redirect URI** has a default value of `/Callback`, which you can leave it as is if there's no special requirement. If you want to change it, you can set the `CallbackPath` property for `LogtoOptions`:

```csharp
builder.Services.AddLogtoAuthentication(options =>
{
  options.CallbackPath = "/SomeOtherCallbackPath";
});
```

Remember to update the value in the Logto application details page accordingly.

> **Note**
> No need to set the **application redirect URI** in the Logto application details page.

### Sign-out

To sign-out with Logto, you can use the `SignOutAsync` method:

```csharp
await HttpContext.SignOutAsync(new AuthenticationProperties
{
  RedirectUri = "/"
});
```

The `SignOutAsync` method will clear the authentication cookie and redirect the user to the Logto sign-out page.

The `RedirectUri` property is used to redirect the user back to your web application after sign-out. Note it is different from the post sign-out redirect URI you configured in the Logto application details page:

1. The post sign-out redirect URI in the Logto application details page is the URI that Logto will redirect the user back to after the user has signed out.
2. The `RedirectUri` property is the URI that will be redirected to after necessary actions have been taken in the Logto authentication middleware.

The order of the actions is 1 -> 2. For clarity, let's call the post sign-out redirect URI in the Logto application details page the **Logto post sign-out redirect URI** and the `RedirectUri` property the **application post sign-out redirect URI**.

The **Logto post sign-out redirect URI** has a default value of `/SignedOutCallback`, which you can leave it as is if there's no special requirement. If you want to change it, you can set the `SignedOutCallbackPath` property for `LogtoOptions`:

```csharp
builder.Services.AddLogtoAuthentication(options =>
{
  options.SignedOutCallbackPath = "/SomeOtherSignedOutCallbackPath";
});
```

Remember to update the value in the Logto application details page accordingly.

> **Note**
> No need to set the **application post sign-out redirect URI** in the Logto application details page.

### Implement sign-in/sign-out buttons (Razor Pages)

First, add the handler methods to your `PageModel`, for example:

```csharp
public class IndexModel : PageModel
{
  public async Task OnPostSignInAsync()
  {
    await HttpContext.ChallengeAsync(new AuthenticationProperties
    {
      RedirectUri = "/"
    });
  }

  public async Task OnPostSignOutAsync()
  {
    await HttpContext.SignOutAsync(new AuthenticationProperties
    {
      RedirectUri = "/"
    });
  }
}
```

Then, add the buttons to your Razor page:

```html
<p>Is authenticated: @User.Identity?.IsAuthenticated</p>
<form method="post">
  @if (User.Identity?.IsAuthenticated == true)
  {
    <button type="submit" asp-page-handler="SignOut">Sign out</button>
  } else {
    <button type="submit" asp-page-handler="SignIn">Sign in</button>
  }
</form>
```

It will show the "Sign in" button if the user is not authenticated, and show the "Sign out" button if the user is authenticated.

### Checkpoint: Run the web application

Now you can run the web application and try to sign-in/sign-out with Logto:

1. Open the web application in your browser, you should see "Is authenticated: False" and the "Sign in" button.
2. Click the "Sign in" button, and you should be redirected to the Logto sign-in page.
3. After you have signed in, you should be redirected back to the web application, and you should see "Is authenticated: True" and the "Sign out" button.
4. Click the "Sign out" button, and you should be redirected to the Logto sign-out page, and then redirected back to the web application.

## The user object

To know if the user is authenticated, you can check the `User.Identity?.IsAuthenticated` property.

To get the user profile claims, you can use the `User.Claims` property:

```csharp
var claims = User.Claims;

// Get the user ID
var userId = claims.FirstOrDefault(c => c.Type == LogtoParameters.Claims.Subject)?.Value;
```

See [`LogtoParameters.Claims`](./api/Logto/AspNetCore/Authentication/LogtoParameters/Claims/index.md) for the list of claim names and their meanings.

### Some claims are missing

Please see [Scopes and claims](#scopes-and-claims) for more details.

## Scopes and claims

Both of "scope" and "claim" are terms from the OAuth 2.0 and OpenID Connect (OIDC) specifications. In OIDC, there are some optional [scopes and claims conventions](https://openid.net/specs/openid-connect-core-1_0.html#Claims) to follow. Logto uses these conventions to define the scopes and claims for the ID token.

In short, when you request a scope, you will get the corresponding claims in the ID token. For example, if you request the `email` scope, you will get the `email` and `email_verified` claims in the ID token.

By default, Logto SDK requests three scopes: `openid`, `profile`, and `offline_access`. You can add more scopes when configuring the authentication middleware:

```csharp
builder.Services.AddLogtoAuthentication(options =>
{
  // ...
  options.Scopes = new string[] {
    LogtoParameters.Scopes.Email,
    LogtoParameters.Scopes.Phone
  }
});
```

> **Note**
> For now, there's no way to remove the default scopes without mutating the `scopes` list.

See [`LogtoParameters.Scopes`](./api/Logto/AspNetCore/Authentication/LogtoParameters/Scopes/index.md) for a list of supported scopes and its mapped claims.

### Special ID token claims

Considering performance and the data size, Logto doesn't include all the claims in the ID token, such as `custom_data` which could be a large JSON object. To fetch these claims, you need to set the `GetClaimsFromUserInfoEndpoint` property to `true`:

```csharp
builder.Services.AddLogtoAuthentication(options =>
{
  // ...
  options.GetClaimsFromUserInfoEndpoint = true;
});
```

Currently, the following claims are not included in the ID token:

- `LogtoParameters.Claims.CustomData` (use `LogtoParameters.Scopes.CustomData` to fetch)
- `LogtoParameters.Claims.Identities` (use `LogtoParameters.Scopes.Identities` to fetch)

## API resources

We recommend to read [🔐 Role-Based Access Control (RBAC)](https://docs.logto.io/docs/recipes/rbac/) first to understand the basic concepts of Logto RBAC and how to set up API resources properly.

### Configure Logto client

Once you have set up the API resource, you can add it when configuring the authentication middleware:

```csharp
builder.Services.AddLogtoAuthentication(options =>
{
  // ...
  options.Resource = "https://<your-api-resource-indicator>";
});
```

> **Note**
> The middleware only accepts one API resource due to the limitation of the underlying OpenID Connect authentication handler.

Each API resource has its own permissions (scopes). For example, you can define the `https://shopping.your-app.com/api` resource to have the `read` and `write` permissions, while the `https://payment.your-app.com/api` resource to have the `pay` permission.

To request these permissions, you can add them when configuring the authentication middleware:

```csharp
builder.Services.AddLogtoAuthentication(options =>
{
  // ...
  options.Resource = "https://shopping.your-app.com/api";
  options.Scopes = new string[] {
    "openid",
    "profile",
    "offline_access",
    "read",
    "write"
  };
});
```

You may notice that scopes are defined separately from API resources. This is because [Resource Indicators for OAuth 2.0](https://www.rfc-editor.org/rfc/rfc8707.html) specifies the final scopes for the request will be the cartesian product of all the scopes at all the targets.

> **Note**
> It is fine to request scopes that are not defined in the API resources. For example, you can request the `email` scope even if the API resources don't have the `email` scope available. Unavailable scopes will be safely ignored.

After the successful sign-in, Logto will issue proper scopes to the API resource according to the user's roles.

## Fetch tokens

Sometimes you may need to fetch the access token or ID token for API calls. You can use the `GetTokenAsync` method to fetch the tokens:

```csharp
var accessToken = await HttpContext.GetTokenAsync(LogtoParameters.Tokens.AccessToken);
var idToken = await HttpContext.GetTokenAsync(LogtoParameters.Tokens.IdToken);
```

No need to worry about the token expiration, the authentication middleware will automatically refresh the tokens when necessary.

> **Caution**
> Although the authentication middleware will automatically refresh the tokens, the claims in the user object will not be updated due to the limitation of the underlying OpenID Connect authentication handler.
> This can be resolved once we write our own authentication handler.

Note the access token above is an opaque token for the userinfo endpoint in OpenID Connect, which is not a JWT token. If you have specified the API resource, you need to use `LogtoParameters.Tokens.AccessTokenForResource` to fetch the access token for the API resource:

```csharp
var accessToken = await HttpContext.GetTokenAsync(LogtoParameters.Tokens.AccessTokenForResource);
```

This token will be a JWT token with the API resource as the audience.
