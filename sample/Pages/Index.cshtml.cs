using Logto.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace logto_csharp_sample.Pages;

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

    public async Task OnPostAsync()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            await HttpContext.SignOutAsync(new AuthenticationProperties { RedirectUri = "/" });
        }
        else
        {
            await HttpContext.ChallengeAsync(new AuthenticationProperties { RedirectUri = "/" });
        }
    }
}
