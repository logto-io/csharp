namespace sample_wasm.Pages;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Blorc.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
using sample_wasm.WeatherForecasts;

[Authorize]
public partial class Home : ComponentBase
{
    [Inject]
    public required IUserManager UserManager { get; set; }

    [Inject]
    WeatherForecastHttpClient WeatherForecastHttpClient { get; set; }

    public TimeSpan? SignOutTimeSpan { get; set; }

    public User<Profile>? User { get; set; }

    private WeatherForecast[] forecasts;

    [CascadingParameter]
    protected Task<AuthenticationState>? AuthenticationStateTask { get; set; }

    protected override async Task OnInitializedAsync()
    {
        User = await UserManager.GetUserAsync<User<Profile>>(AuthenticationStateTask!);

        UserManager.UserActivity += OnUserManagerUserActivity;
        UserManager.UserInactivity += OnUserManagerUserInactivity;

        if(User is not null)
        { 
            forecasts = await WeatherForecastHttpClient.GetForecastAsync();
        }

    }

    private void OnUserManagerUserInactivity(object? sender, UserInactivityEventArgs args)
    {
        SignOutTimeSpan = args.SignOutTimeSpan;
        StateHasChanged();
    }

    private void OnUserManagerUserActivity(object? sender, UserActivityEventArgs args)
    {
        SignOutTimeSpan = null;
        StateHasChanged();
    }

    private async Task OnLoginButtonClickAsync(MouseEventArgs obj)
    {
        await UserManager.SignInRedirectAsync();
    }

    private async Task OnLogoutButtonClickAsync(MouseEventArgs obj)
    {
        await UserManager.SignOutRedirectAsync();
    }

    public void Dispose()
    {
        UserManager.UserActivity -= OnUserManagerUserActivity;
        UserManager.UserInactivity -= OnUserManagerUserInactivity;
    }
}
