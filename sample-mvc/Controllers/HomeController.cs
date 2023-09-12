using Logto.AspNetCore.Authentication;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using sample_mvc.Models;

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
        return Challenge(new AuthenticationProperties { RedirectUri = "/" });
    }

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
