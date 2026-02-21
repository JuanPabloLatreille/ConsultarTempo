using System.Text.Json.Serialization;

namespace Infraestructure.Services.ProvedorClima.Responses;

public class OpenWeatherResponse
{
    [JsonPropertyName("main")]
    public MainInfo Main { get; set; } = null!;

    [JsonPropertyName("coord")]
    public CoordInfo Coord { get; set; } = null!;

    [JsonPropertyName("sys")]
    public SysInfo Sys { get; set; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public class MainInfo
{
    [JsonPropertyName("temp")]
    public double Temp { get; set; }
}

public class CoordInfo
{
    [JsonPropertyName("lat")]
    public double Lat { get; set; }

    [JsonPropertyName("lon")]
    public double Lon { get; set; }
}

public class SysInfo
{
    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;
}