using Domain.Entities.Cidades;
using Domain.Entities.HistoricosTemperaturas;
using Domain.Interfaces;
using Domain.Interfaces.Cidades;
using Domain.Interfaces.HistoricosTemperaturas;
using Domain.Interfaces.Services;
using MediatR;

namespace Application.HistoricosTemperaturas.Commands.CadastrarHistoricoTemperatura;

public sealed record CadastrarHistoricoTemperaturaCommand(
    string NomeCidade) : IRequest<CadastrarHistoricoTemperaturaResult>;

public sealed class CadastrarHistoricoTemperaturaCommandHandler
    : IRequestHandler<CadastrarHistoricoTemperaturaCommand, CadastrarHistoricoTemperaturaResult>
{
    private readonly ICidadeRepository _cidadeRepository;

    private readonly IHistoricoTemperaturaRepository _historicoRepository;

    private readonly IUnitOfWork _unitOfWork;

    private readonly IProvedorClimaService _provedorClimaService;

    public CadastrarHistoricoTemperaturaCommandHandler(
        ICidadeRepository cidadeRepository,
        IHistoricoTemperaturaRepository historicoRepository,
        IUnitOfWork unitOfWork,
        IProvedorClimaService provedorClimaService)
    {
        _cidadeRepository = cidadeRepository;
        _historicoRepository = historicoRepository;
        _unitOfWork = unitOfWork;
        _provedorClimaService = provedorClimaService;
    }

    public async Task<CadastrarHistoricoTemperaturaResult> Handle(
        CadastrarHistoricoTemperaturaCommand request, CancellationToken cancellationToken)
    {
        var resultadoClima = await _provedorClimaService.ObterTemperaturaAsync(request.NomeCidade, cancellationToken);

        var cidade = await _cidadeRepository.ObterPorNomeAsync(request.NomeCidade, cancellationToken);

        if (cidade is null)
        {
            cidade = Cidade.Criar(
                resultadoClima.NomeCidade,
                resultadoClima.CodigoPais,
                resultadoClima.Latitude,
                resultadoClima.Longitude);

            await _cidadeRepository.AdicionarAsync(cidade, cancellationToken);
        }

        var historico = HistoricoTemperatura.Criar(resultadoClima.Temperatura, DateTime.UtcNow, cidade);

        await _historicoRepository.AdicionarAsync(historico, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CadastrarHistoricoTemperaturaResult(
            historico.Id,
            cidade.Id,
            cidade.Nome,
            historico.Temperatura,
            historico.DataRegistro);
    }
}