# Logto ASP.NET Web API sample project

This sample project shows how to use LogTo when proecting an API.
You can use it with the sample-wasm project example!

## Prerequisites

- .NET 6.0 or higher
- A [Logto Cloud](https://logto.io/) account or a self-hosted Logto
- A Logto single-page application created
- Set up an API resource in Logto

If you don't have the Logto application created, please follow the [⚡ Get started](https://docs.logto.io/docs/tutorials/get-started/) guide to create one.

## Configuration

Update the `appsettings.Development.json` of the wasm sample with the following structure:

```jsonc
{
  // ...
  "IdentityServer": {
  //...
    "Scope": "... scope-permision-name" // Add more scopes if needed
    "Resource": "<your-api-resource-url>",
    "ExtraTokenParams": {
      "resource": "<your-api-resource-url>" // Ensure the key is lowercase
    }
  }
  }
}
```

## Run the sample

```bash
dotnet run # or `dotnet watch` to run in watch mode
```
