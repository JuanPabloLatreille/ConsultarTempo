namespace Application.Cidades.Commands.CadastrarCidade;

public sealed record CadastrarCidadeResult(
    Guid Id,
    string Nome,
    string Uf,
    double Latitude,
    double Longitude,
    DateTime CriadoEm
);