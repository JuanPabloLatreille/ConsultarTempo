using Domain.Models.ResultadoClima;

namespace Domain.Interfaces.Services;

public interface IProvedorClimaService
{
    Task<ResultadoClimaModel> ObterTemperaturaAsync(string nomeCidade, CancellationToken cancellationToken = default);

    Task<ResultadoClimaModel> ObterTemperaturaPorCoordenadasAsync(
        double latitude,
        double longitude,
        CancellationToken cancellationToken = default);
}