namespace Application.HistoricosTemperaturas.Queries.ConsultarTodosHistoricos;

public sealed record ConsultarTodosHistoricosQueryResult(
    Guid Id,
    string NomeCidade,
    double Temperatura,
    DateTime DataRegistro);