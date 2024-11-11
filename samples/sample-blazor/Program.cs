using sample_blazor.Components;
using Logto.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddLogtoAuthentication(options =>
{
    options.Endpoint = builder.Configuration["Logto:Endpoint"]!;
    options.AppId = builder.Configuration["Logto:AppId"]!;
    options.AppSecret = builder.Configuration["Logto:AppSecret"];
    options.Scopes = [
        LogtoParameters.Scopes.Email,
        LogtoParameters.Scopes.Phone,
        LogtoParameters.Scopes.CustomData,
        LogtoParameters.Scopes.Identities
    ];
    options.Resource = builder.Configuration["Logto:Resource"];
    options.GetClaimsFromUserInfoEndpoint = true;
});
builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapGet("/SignIn", async context =>
{
    if (!(context.User?.Identity?.IsAuthenticated ?? false))
    {
        var authProperties = new AuthenticationProperties 
        { 
            RedirectUri = "/" 
        };

        authProperties.SetParameter("first_screen", LogtoParameters.Authentication.FirstScreen.Register);
        authProperties.SetParameter("identifiers", string.Join(",", new[] 
        { 
            LogtoParameters.Authentication.Identifiers.Username,
        }));

        // Set `direct_sign_in`
        var directSignIn = new LogtoParameters.Authentication.DirectSignIn
        {
            Target = "github",
            Method = LogtoParameters.Authentication.DirectSignIn.Methods.Social
        };
        authProperties.SetParameter("direct_sign_in", System.Text.Json.JsonSerializer.Serialize(directSignIn));

        // Set `extra_params`
        var extraParams = new LogtoParameters.Authentication.ExtraParams
        {
            { "utm_source", "website" },
            { "utm_medium", "organic" }
        };
        authProperties.SetParameter("extra_params", System.Text.Json.JsonSerializer.Serialize(extraParams));

        await context.ChallengeAsync(authProperties);
    } 
    else 
    {
        context.Response.Redirect("/");
    }
});

app.MapGet("/SignOut", async context =>
{
    if (context.User?.Identity?.IsAuthenticated ?? false)
    {
        await context.SignOutAsync(new AuthenticationProperties { RedirectUri = "/" });
    } else {
        context.Response.Redirect("/");
    }
});

app.Run();
