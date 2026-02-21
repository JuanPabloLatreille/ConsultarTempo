namespace Application.HistoricosTemperaturas.Queries.ConsultarHistoricoPorNomeCidade;

public sealed record ConsultarHistoricoPorNomeCidadeQueryResult(
    Guid Id,
    string NomeCidade,
    double Temperatura,
    DateTime DataRegistro);