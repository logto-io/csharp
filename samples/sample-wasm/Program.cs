using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blorc.OpenIdConnect;
using Blorc.Services;
using sample_wasm;
using sample_wasm.WeatherForecasts;

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

builder.Services
    .AddHttpClient<WeatherForecastHttpClient>(client => client.BaseAddress = new Uri("http://localhost:5150/"))
    .AddAccessToken(); // to get a token that will be sent to the API

var webAssemblyHost = builder.Build();

await webAssemblyHost
    .ConfigureDocumentAsync(async documentService =>
    {
        await documentService.InjectBlorcCoreJsAsync();
        await documentService.InjectOpenIdConnectAsync();
    });

await webAssemblyHost.RunAsync();
