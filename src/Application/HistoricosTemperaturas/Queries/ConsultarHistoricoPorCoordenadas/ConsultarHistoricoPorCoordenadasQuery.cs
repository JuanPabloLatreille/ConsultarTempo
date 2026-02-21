using Domain.Exceptions;
using Domain.Interfaces.Cidades;
using Domain.Interfaces.HistoricosTemperaturas;
using MediatR;
using System.Net;

namespace Application.HistoricosTemperaturas.Queries.ConsultarHistoricoPorCoordenadas;

public sealed record ConsultarHistoricoPorCoordenadasQuery(
    double Latitude,
    double Longitude) : IRequest<List<ConsultarHistoricoPorCoordenadasQueryResult>>;

public sealed class ConsultarHistoricoPorCoordenadasQueryHandler
    : IRequestHandler<ConsultarHistoricoPorCoordenadasQuery, List<ConsultarHistoricoPorCoordenadasQueryResult>>
{
    private readonly ICidadeRepository _cidadeRepository;

    private readonly IHistoricoTemperaturaRepository _historicoRepository;

    public ConsultarHistoricoPorCoordenadasQueryHandler(
        ICidadeRepository cidadeRepository,
        IHistoricoTemperaturaRepository historicoRepository)
    {
        _cidadeRepository = cidadeRepository;
        _historicoRepository = historicoRepository;
    }

    public async Task<List<ConsultarHistoricoPorCoordenadasQueryResult>> Handle(
        ConsultarHistoricoPorCoordenadasQuery request, CancellationToken cancellationToken)
    {
        var cidade = await _cidadeRepository.ObterPorCoordenadasAsync(
            request.Latitude, request.Longitude, cancellationToken);

        if (cidade is null)
            throw new ValidacaoException(
                ["Nenhuma cidade encontrada para as coordenadas informadas."],
                HttpStatusCode.NotFound);

        var historicos = await _historicoRepository.ObterPorCidadeIdUltimos30DiasAsync(
            cidade.Id, cancellationToken);

        return historicos
            .Select(h => new ConsultarHistoricoPorCoordenadasQueryResult(
                h.Id,
                cidade.Nome,
                h.Temperatura,
                h.DataRegistro))
            .ToList();
    }
}