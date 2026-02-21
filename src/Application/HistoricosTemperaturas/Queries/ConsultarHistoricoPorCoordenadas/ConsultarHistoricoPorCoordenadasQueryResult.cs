namespace Application.HistoricosTemperaturas.Queries.ConsultarHistoricoPorCoordenadas;

public sealed record ConsultarHistoricoPorCoordenadasQueryResult(
    Guid Id,
    string NomeCidade,
    double Temperatura,
    DateTime DataRegistro);