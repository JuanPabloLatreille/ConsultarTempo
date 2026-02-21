using Domain.Interfaces.Cidades;
using MediatR;

namespace Application.Cidades.Queries.ConsultarTodasCidades;

public sealed record ConsultarTodasCidadesQuery : IRequest<List<ConsultarTodasCidadesQueryResult>>;

public class
    ConsultarTodasCidadesQueryHandler : IRequestHandler<ConsultarTodasCidadesQuery,
    List<ConsultarTodasCidadesQueryResult>>
{
    private readonly ICidadeRepository _cidadeRepository;

    public ConsultarTodasCidadesQueryHandler(ICidadeRepository cidadeRepository)
    {
        _cidadeRepository = cidadeRepository;
    }

    public async Task<List<ConsultarTodasCidadesQueryResult>> Handle(ConsultarTodasCidadesQuery request,
        CancellationToken cancellationToken)
    {
        var cidades = await _cidadeRepository.ObterTodosAsync(cancellationToken);
        return cidades.Select(c => new ConsultarTodasCidadesQueryResult(
            c.Id,
            c.Nome,
            c.Uf,
            c.Latitude,
            c.Longitude,
            c.CriadoEm)).ToList();
    }
}