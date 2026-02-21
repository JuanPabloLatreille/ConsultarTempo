namespace Application.Cidades.Queries.ConsultarCidadePorId;

public sealed record ConsultarCidadePorIdQueryResult(
    Guid Id,
    string Nome,
    string Uf,
    double Latitude,
    double Longitude,
    DateTime CriadoEm);