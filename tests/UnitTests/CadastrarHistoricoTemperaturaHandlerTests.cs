using Application.HistoricosTemperaturas.Commands.CadastrarHistoricoTemperatura;
using Bogus;
using Domain.Entities.Cidades;
using Domain.Entities.HistoricosTemperaturas;
using Domain.Interfaces;
using Domain.Interfaces.Cidades;
using Domain.Interfaces.HistoricosTemperaturas;
using Domain.Interfaces.Services;
using Domain.Models.ResultadoClima;
using FluentAssertions;
using Moq;

namespace UnitTests;

[TestFixture]
public class CadastrarHistoricoTemperaturaHandlerTests
{
    private Mock<ICidadeRepository> _cidadeRepositoryMock;

    private Mock<IHistoricoTemperaturaRepository> _historicoRepositoryMock;

    private Mock<IUnitOfWork> _unitOfWorkMock;

    private Mock<IProvedorClimaService> _provedorClimaMock;

    private CadastrarHistoricoTemperaturaCommandHandler _handler;

    private Faker _faker;

    [SetUp]
    public void SetUp()
    {
        _cidadeRepositoryMock = new Mock<ICidadeRepository>();
        _historicoRepositoryMock = new Mock<IHistoricoTemperaturaRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _provedorClimaMock = new Mock<IProvedorClimaService>();
        _faker = new Faker("pt_BR");

        _handler = new CadastrarHistoricoTemperaturaCommandHandler(
            _cidadeRepositoryMock.Object,
            _historicoRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _provedorClimaMock.Object);
    }

    [Test]
    public async Task Handle_CidadeExistente_DeveRegistrarHistoricoSemCriarCidade()
    {
        // Arrange
        var nomeCidade = _faker.Address.City();
        var temperatura = _faker.Random.Double(-10, 45);
        var latitude = _faker.Address.Latitude();
        var longitude = _faker.Address.Longitude();

        var cidade = Cidade.Criar(nomeCidade, "BR", latitude, longitude);
        var command = new CadastrarHistoricoTemperaturaCommand(nomeCidade);

        _provedorClimaMock
            .Setup(p => p.ObterTemperaturaAsync(nomeCidade, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ResultadoClimaModel(nomeCidade, "BR", temperatura, latitude, longitude));

        _cidadeRepositoryMock
            .Setup(r => r.ObterPorNomeAsync(nomeCidade, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cidade);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var resultado = await _handler.Handle(command, CancellationToken.None);

        // Assert
        resultado.Should().NotBeNull();
        resultado.NomeCidade.Should().Be(nomeCidade);
        resultado.TemperaturaAtual.Should().Be(temperatura);

        _historicoRepositoryMock.Verify(
            r => r.AdicionarAsync(It.IsAny<HistoricoTemperatura>(), It.IsAny<CancellationToken>()),
            Times.Once);

        _cidadeRepositoryMock.Verify(
            r => r.AdicionarAsync(It.IsAny<Cidade>(), It.IsAny<CancellationToken>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task Handle_CidadeNaoExistente_DeveCriarCidadeERegistrarHistorico()
    {
        // Arrange
        var nomeCidade = _faker.Address.City();
        var temperatura = _faker.Random.Double(-10, 45);
        var latitude = _faker.Address.Latitude();
        var longitude = _faker.Address.Longitude();

        var command = new CadastrarHistoricoTemperaturaCommand(nomeCidade);

        _provedorClimaMock
            .Setup(p => p.ObterTemperaturaAsync(nomeCidade, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ResultadoClimaModel(nomeCidade, "BR", temperatura, latitude, longitude));

        _cidadeRepositoryMock
            .Setup(r => r.ObterPorNomeAsync(nomeCidade, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Cidade?)null);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var resultado = await _handler.Handle(command, CancellationToken.None);

        // Assert
        resultado.Should().NotBeNull();
        resultado.NomeCidade.Should().Be(nomeCidade);
        resultado.TemperaturaAtual.Should().Be(temperatura);

        _cidadeRepositoryMock.Verify(
            r => r.AdicionarAsync(It.IsAny<Cidade>(), It.IsAny<CancellationToken>()),
            Times.Once);

        _historicoRepositoryMock.Verify(
            r => r.AdicionarAsync(It.IsAny<HistoricoTemperatura>(), It.IsAny<CancellationToken>()),
            Times.Once);

        _unitOfWorkMock.Verify(
            u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task Handle_DeveConsultarProvedorClimaComNomeCidadeCorreto()
    {
        // Arrange
        var nomeCidade = _faker.Address.City();

        var command = new CadastrarHistoricoTemperaturaCommand(nomeCidade);

        _provedorClimaMock
            .Setup(p => p.ObterTemperaturaAsync(nomeCidade, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ResultadoClimaModel(nomeCidade, "BR", 20.0, -23.0, -46.0));

        _cidadeRepositoryMock
            .Setup(r => r.ObterPorNomeAsync(nomeCidade, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Cidade?)null);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _provedorClimaMock.Verify(
            p => p.ObterTemperaturaAsync(nomeCidade, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}