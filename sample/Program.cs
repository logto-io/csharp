using Microsoft.AspNetCore.Authentication;
using Logto.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

static void CheckSameSite(HttpContext httpContext, CookieOptions options)
{
    if (options.SameSite == SameSiteMode.None && options.Secure == false)
    {
        options.SameSite = SameSiteMode.Unspecified;
    }
}

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
    options.OnAppendCookie = cookieContext => CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
    options.OnDeleteCookie = cookieContext => CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = LogtoDefaults.CookieScheme;
    options.DefaultChallengeScheme = LogtoDefaults.AuthenticationScheme;
    options.DefaultSignOutScheme = LogtoDefaults.AuthenticationScheme;
})
.AddLogto(options =>
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
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseCookiePolicy();

app.UseAuthentication();
// app.UseAuthorization();

app.MapRazorPages();

// Print debug info when receiving a request.
app.Use(async (context, next) =>
{
    Console.Write("Request: ");
    Console.Write(context.Request.Method + " ");
    Console.Write(context.Request.Path);
    Console.WriteLine();

    await next.Invoke();
});

app.Run();
