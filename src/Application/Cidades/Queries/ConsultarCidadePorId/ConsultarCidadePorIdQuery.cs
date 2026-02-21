using System.Net;
using Domain.Exceptions;
using Domain.Interfaces.Cidades;
using MediatR;

namespace Application.Cidades.Queries.ConsultarCidadePorId;

public sealed record ConsultarCidadePorIdQuery(Guid Id) : IRequest<ConsultarCidadePorIdQueryResult>;

public sealed class ConsultarCidadePorIdQueryHandler
    : IRequestHandler<ConsultarCidadePorIdQuery, ConsultarCidadePorIdQueryResult>
{
    private readonly ICidadeRepository _cidadeRepository;

    public ConsultarCidadePorIdQueryHandler(ICidadeRepository cidadeRepository)
    {
        _cidadeRepository = cidadeRepository;
    }

    public async Task<ConsultarCidadePorIdQueryResult> Handle(
        ConsultarCidadePorIdQuery request, CancellationToken cancellationToken)
    {
        var cidade = await _cidadeRepository.ObterPorIdAsync(request.Id, cancellationToken);

        if (cidade is null)
            throw new ValidacaoException(
                ["Cidade não encontrada."],
                HttpStatusCode.NotFound);

        return new ConsultarCidadePorIdQueryResult(
            cidade.Id,
            cidade.Nome,
            cidade.Uf,
            cidade.Latitude,
            cidade.Longitude,
            cidade.CriadoEm);
    }
}