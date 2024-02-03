# Logto ASP.NET Core sample project for MVC

This sample project shows how to use the [Logto ASP.NET Core authentication middleware](../src/Logto.AspNetCore.Authentication/) to authenticate users with Logto in a Blazor Server application.

## Prerequisites

- .NET 8.0 or higher (This sample is created with the [new Blazor template](https://learn.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-8.0#new-blazor-web-app-template) in .NET 8.0)
- A [Logto Cloud](https://logto.io/) account or a self-hosted Logto
- A Logto traditional web application created

### Optional

- Set up an API resource in Logto

If you don't have the Logto application created, please follow the [⚡ Get started](https://docs.logto.io/docs/tutorials/get-started/) guide to create one.

## Configuration

Create an `appsettings.Development.json` (or `appsettings.json`) with the following structure:

```jsonc
{
  // ...
  "Logto": {
    "Endpoint": "https://<your-logto-endpoint>/",
    "AppId": "<your-logto-app-id>",
    "AppSecret": "<your-logto-app-secret>"
  }
}
```

If you need to test API resource, add the `Resource` key:

```jsonc
{
  // ...
  "Logto": {
    // ...
    "Resource": "https://<your-api-resource-indicator>"
  }
}
```

## Run the sample

```bash
dotnet run # or `dotnet watch` to run in watch mode
```
