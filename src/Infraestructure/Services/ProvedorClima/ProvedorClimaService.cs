using Domain.Exceptions;
using Domain.Interfaces.Services;
using Domain.Models.ResultadoClima;
using Infraestructure.Services.ProvedorClima.Options;
using Infraestructure.Services.ProvedorClima.Responses;
using Microsoft.Extensions.Options;
using RestSharp;

namespace Infraestructure.Services.ProvedorClima;

public class ProvedorClimaService : IProvedorClimaService
{
    private readonly RestClient _client;

    private readonly string _apiKey;

    public ProvedorClimaService(IOptions<OpenWeatherMapOptions> options)
    {
        _client = new RestClient("https://api.openweathermap.org/data/2.5");
        _apiKey = options.Value.ApiKey;
    }

    public async Task<ResultadoClimaModel> ObterTemperaturaAsync(
        string nomeCidade,
        string codigoPais,
        CancellationToken cancellationToken = default)
    {
        var request = new RestRequest("weather")
            .AddQueryParameter("q", $"{nomeCidade},{codigoPais}")
            .AddQueryParameter("units", "metric")
            .AddQueryParameter("appid", _apiKey);

        var response = await _client.GetAsync<OpenWeatherResponse>(request, cancellationToken);

        if (response?.Main is null)
            throw new ValidacaoException(
                ["Não foi possível obter a temperatura da cidade informada."]);

        return new ResultadoClimaModel(
            response.Name,
            response.Sys.Country,
            response.Main.Temp,
            response.Coord.Lat,
            response.Coord.Lon);
    }

    public async Task<ResultadoClimaModel> ObterTemperaturaPorCoordenadasAsync(double latitude, double longitude,
        CancellationToken cancellationToken = default)
    {
        var request = new RestRequest("weather")
            .AddQueryParameter("lat", latitude)
            .AddQueryParameter("lon", longitude)
            .AddQueryParameter("units", "metric")
            .AddQueryParameter("appid", _apiKey);

        var response = await _client.GetAsync<OpenWeatherResponse>(request, cancellationToken);

        if (response?.Main is null)
            throw new Exception("Não foi possível obter a temperatura para as coordenadas informadas.");

        return new ResultadoClimaModel(
            response.Name,
            response.Sys.Country,
            response.Main.Temp,
            response.Coord.Lat,
            response.Coord.Lon);
    }
}