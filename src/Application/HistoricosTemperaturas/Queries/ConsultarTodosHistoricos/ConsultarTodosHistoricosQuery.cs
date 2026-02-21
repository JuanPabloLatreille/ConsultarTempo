using Domain.Interfaces.HistoricosTemperaturas;
using MediatR;

namespace Application.HistoricosTemperaturas.Queries.ConsultarTodosHistoricos;

public sealed record ConsultarTodosHistoricosQuery : IRequest<List<ConsultarTodosHistoricosQueryResult>>;

public sealed class ConsultarTodosHistoricosQueryHandler : IRequestHandler<ConsultarTodosHistoricosQuery, List<ConsultarTodosHistoricosQueryResult>>
{
    private readonly IHistoricoTemperaturaRepository _historicoRepository;

    public ConsultarTodosHistoricosQueryHandler(IHistoricoTemperaturaRepository historicoRepository)
    {
        _historicoRepository = historicoRepository;
    }

    public async Task<List<ConsultarTodosHistoricosQueryResult>> Handle(ConsultarTodosHistoricosQuery request, CancellationToken cancellationToken)
    {
        var listaHistoricosTemperaturas = await _historicoRepository.ObterTodosAsync(cancellationToken);
        
        return listaHistoricosTemperaturas.Select(historico => new ConsultarTodosHistoricosQueryResult(
            historico.Id,
            historico.Cidade.Nome,
            historico.Temperatura,
            historico.DataRegistro)).ToList();
    }
}