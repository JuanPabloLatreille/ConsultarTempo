namespace Application.HistoricosTemperaturas.Commands.CadastrarHistoricoTemperaturaPorCoordenadas;

public sealed record CadastrarHistoricoTemperaturaPorCoordenadasResult(
    Guid Id,
    Guid CidadeId,
    string NomeCidade,
    double TemperaturaAtual,
    DateTime DataRegistro);