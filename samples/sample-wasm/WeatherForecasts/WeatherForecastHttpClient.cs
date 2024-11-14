using System.Net.Http.Json;

namespace sample_wasm.WeatherForecasts;

public class WeatherForecastHttpClient
{
    private readonly HttpClient _http;

    public WeatherForecastHttpClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<WeatherForecast[]> GetForecastAsync()
    {
        var forecasts = Array.Empty<WeatherForecast>();

        try
        {
            forecasts = await _http.GetFromJsonAsync<WeatherForecast[]>("WeatherForecast");
        }
        catch
        {
            // ignored
        }

        return forecasts;
    }
}
