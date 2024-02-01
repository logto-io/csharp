using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blorc.OpenIdConnect;
using Blorc.Services;
using sample_wasm;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddBlorcCore();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlorcOpenIdConnect(
    options =>
    {
        builder.Configuration.Bind("IdentityServer", options);
    });

var webAssemblyHost = builder.Build();

await webAssemblyHost
    .ConfigureDocumentAsync(async documentService =>
    {
        await documentService.InjectBlorcCoreJsAsync();
        await documentService.InjectOpenIdConnectAsync();
    });


await webAssemblyHost.RunAsync();
