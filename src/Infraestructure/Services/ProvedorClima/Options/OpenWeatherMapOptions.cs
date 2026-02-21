namespace Infraestructure.Services.ProvedorClima.Options;

public class OpenWeatherMapOptions
{
    public const string SectionName = "OpenWeatherMap";

    public string ApiKey { get; set; } = string.Empty;
}