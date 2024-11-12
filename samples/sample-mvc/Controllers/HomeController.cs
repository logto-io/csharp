using Logto.AspNetCore.Authentication;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using sample_mvc.Models;
using System.Text.Json;

namespace sample_mvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var logtoOptions = HttpContext.GetLogtoOptions();
        ViewData["Resource"] = logtoOptions.Resource;
        ViewData["AccessTokenForResource"] = await HttpContext.GetTokenAsync(LogtoParameters.Tokens.AccessTokenForResource);
        return View();
    }

    public IActionResult SignIn()
    {
        var authProperties = new AuthenticationProperties 
        { 
            RedirectUri = "/" 
        };

        // Set the first screen, see https://docs.logto.io/docs/references/openid-connect/authentication-parameters/#first-screen.
        authProperties.SetParameter("first_screen", LogtoParameters.Authentication.FirstScreen.Register);
        // Set the identifiers, should work with `first_screen`.
        authProperties.SetParameter("identifiers", string.Join(",", new[] 
        { 
            LogtoParameters.Authentication.Identifiers.Username,
        }));

        var directSignIn = new LogtoParameters.Authentication.DirectSignIn
        {
            Target = "github",
            Method = LogtoParameters.Authentication.DirectSignIn.Methods.Social
        };
        // Set the direct sign-in, see https://docs.logto.io/docs/references/openid-connect/authentication-parameters/#direct-sign-in.
        authProperties.SetParameter("direct_sign_in", JsonSerializer.Serialize(directSignIn));

        return Challenge(authProperties);
    }

    // Use the `new` keyword to avoid conflict with the `ControllerBase.SignOut` method
    new public IActionResult SignOut()
    {
        return SignOut(new AuthenticationProperties { RedirectUri = "/" });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
