using System.Net;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Interfaces.HistoricosTemperaturas;
using MediatR;

namespace Application.HistoricosTemperaturas.Commands.RemoverHistoricoTemperatura;

public sealed record RemoverHistoricoTemperaturaCommand(Guid Id) : IRequest;

public sealed class RemoverHistoricoTemperaturaCommandHandler : IRequestHandler<RemoverHistoricoTemperaturaCommand>
{
    private readonly IHistoricoTemperaturaRepository _historicoRepository;

    private readonly IUnitOfWork _unitOfWork;

    public RemoverHistoricoTemperaturaCommandHandler(IHistoricoTemperaturaRepository historicoRepository,
        IUnitOfWork unitOfWork)
    {
        _historicoRepository = historicoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RemoverHistoricoTemperaturaCommand request, CancellationToken cancellationToken)
    {
        var historico = await _historicoRepository.ObterPorIdAsync(request.Id, cancellationToken);

        if (historico is null)
            throw new ValidacaoException(["Histórico não encontrado"], HttpStatusCode.NotFound);

        _historicoRepository.Remover(historico);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}