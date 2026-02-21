using Domain.Entities.HistoricosTemperaturas;

namespace Domain.Interfaces.HistoricosTemperaturas;

public interface IHistoricoTemperaturaRepository
{
    Task<HistoricoTemperatura?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<HistoricoTemperatura>> ObterPorCidadeIdAsync(Guid cidadeId, CancellationToken cancellationToken = default);

    Task<List<HistoricoTemperatura>> ObterPorCidadeIdUltimos30DiasAsync(Guid cidadeId, CancellationToken cancellationToken = default);

    Task AdicionarAsync(HistoricoTemperatura historico, CancellationToken cancellationToken = default);
}