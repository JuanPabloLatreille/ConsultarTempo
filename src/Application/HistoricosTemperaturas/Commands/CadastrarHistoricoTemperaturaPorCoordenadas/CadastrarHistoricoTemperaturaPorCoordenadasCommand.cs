using Domain.Entities.Cidades;
using Domain.Entities.HistoricosTemperaturas;
using Domain.Interfaces;
using Domain.Interfaces.Cidades;
using Domain.Interfaces.HistoricosTemperaturas;
using Domain.Interfaces.Services;
using MediatR;

namespace Application.HistoricosTemperaturas.Commands.CadastrarHistoricoTemperaturaPorCoordenadas;

public sealed record CadastrarHistoricoTemperaturaPorCoordenadasCommand(
    double Latitude,
    double Longitude) : IRequest<CadastrarHistoricoTemperaturaPorCoordenadasResult>;

public sealed class CadastrarHistoricoTemperaturaPorCoordenadasCommandHandler
    : IRequestHandler<CadastrarHistoricoTemperaturaPorCoordenadasCommand,
        CadastrarHistoricoTemperaturaPorCoordenadasResult>
{
    private readonly ICidadeRepository _cidadeRepository;

    private readonly IHistoricoTemperaturaRepository _historicoRepository;

    private readonly IUnitOfWork _unitOfWork;

    private readonly IProvedorClimaService _provedorClimaService;

    public CadastrarHistoricoTemperaturaPorCoordenadasCommandHandler(
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

    public async Task<CadastrarHistoricoTemperaturaPorCoordenadasResult> Handle(
        CadastrarHistoricoTemperaturaPorCoordenadasCommand request, CancellationToken cancellationToken)
    {
        var resultadoClima = await _provedorClimaService.ObterTemperaturaPorCoordenadasAsync(
            request.Latitude, request.Longitude, cancellationToken);

        var cidade = await _cidadeRepository.ObterPorNomeAsync(resultadoClima.NomeCidade, cancellationToken);

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

        return new CadastrarHistoricoTemperaturaPorCoordenadasResult(
            historico.Id,
            cidade.Id,
            cidade.Nome,
            historico.Temperatura,
            historico.DataRegistro);
    }
}