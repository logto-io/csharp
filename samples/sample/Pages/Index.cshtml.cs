﻿using Logto.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace sample.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public async Task OnGetAsync()
    {
        var logtoOptions = HttpContext.GetLogtoOptions();
        ViewData["Resource"] = logtoOptions.Resource;
        ViewData["AccessTokenForResource"] = await HttpContext.GetTokenAsync(LogtoParameters.Tokens.AccessTokenForResource);
    }

    public async Task OnPostSignInAsync()
    {
        await HttpContext.ChallengeAsync(new AuthenticationProperties { RedirectUri = "/" });
    }

    public async Task OnPostSignOutAsync()
    {
        await HttpContext.SignOutAsync(new AuthenticationProperties { RedirectUri = "/" });
    }
}
