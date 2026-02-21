using Domain.Exceptions;
using Domain.Interfaces.Cidades;
using Domain.Interfaces.HistoricosTemperaturas;
using MediatR;
using System.Net;

namespace Application.HistoricosTemperaturas.Queries.ConsultarHistoricoPorNomeCidade;

public sealed record ConsultarHistoricoPorNomeCidadeQuery(
    string NomeCidade) : IRequest<List<ConsultarHistoricoPorNomeCidadeQueryResult>>;

public sealed class ConsultarHistoricoPorNomeCidadeQueryHandler
    : IRequestHandler<ConsultarHistoricoPorNomeCidadeQuery, List<ConsultarHistoricoPorNomeCidadeQueryResult>>
{
    private readonly ICidadeRepository _cidadeRepository;

    private readonly IHistoricoTemperaturaRepository _historicoRepository;

    public ConsultarHistoricoPorNomeCidadeQueryHandler(
        ICidadeRepository cidadeRepository,
        IHistoricoTemperaturaRepository historicoRepository)
    {
        _cidadeRepository = cidadeRepository;
        _historicoRepository = historicoRepository;
    }

    public async Task<List<ConsultarHistoricoPorNomeCidadeQueryResult>> Handle(
        ConsultarHistoricoPorNomeCidadeQuery request, CancellationToken cancellationToken)
    {
        var cidade = await _cidadeRepository.ObterPorNomeAsync(request.NomeCidade, cancellationToken);

        if (cidade is null)
            throw new ValidacaoException(
                ["Cidade não encontrada."],
                HttpStatusCode.NotFound);

        var historicos = await _historicoRepository.ObterPorCidadeIdUltimos30DiasAsync(
            cidade.Id, cancellationToken);

        return historicos
            .Select(h => new ConsultarHistoricoPorNomeCidadeQueryResult(
                h.Id,
                cidade.Nome,
                h.Temperatura,
                h.DataRegistro))
            .ToList();
    }
}