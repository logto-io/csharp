using sample_blazor.Components;
using Logto.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddLogtoAuthentication(options =>
{
    options.Endpoint = builder.Configuration["Logto:Endpoint"]!;
    options.AppId = builder.Configuration["Logto:AppId"]!;
    options.AppSecret = builder.Configuration["Logto:AppSecret"];
    options.Scopes = new string[] {
        LogtoParameters.Scopes.Email,
        LogtoParameters.Scopes.Phone,
        LogtoParameters.Scopes.CustomData,
        LogtoParameters.Scopes.Identities
    };
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
        await context.ChallengeAsync(new AuthenticationProperties { RedirectUri = "/" });
    } else {
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
