namespace Application.HistoricosTemperaturas.Commands.CadastrarHistoricoTemperatura;

public sealed record CadastrarHistoricoTemperaturaResult(
    Guid Id,
    Guid CidadeId,
    string NomeCidade,
    double TemperaturaAtual,
    DateTime DataRegistro);