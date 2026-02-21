namespace Application.Cidades.Queries.ConsultarTodasCidades;

public sealed record ConsultarTodasCidadesQueryResult(
    Guid Id,
    string Nome,
    string Uf,
    double Latitude,
    double Longitude,
    DateTime CriadoEm);