using Domain.Entities.Cidades;

namespace Domain.Entities.HistoricosTemperaturas;

public sealed class HistoricoTemperatura : EntidadeBase
{
    private HistoricoTemperatura()
    {
        // Construtor privado para uso do Entity Framework
    }

    private HistoricoTemperatura(double temperatura, DateTime dataRegistro, Cidade cidade)
    {
        Temperatura = temperatura;
        DataRegistro = dataRegistro;
        Cidade = cidade;
        CidadeId = cidade.Id;
    }

    public double Temperatura { get; private set; }

    public DateTime DataRegistro { get; private set; }

    public Cidade Cidade { get; private set; }

    public Guid CidadeId { get; private set; }

    public static HistoricoTemperatura Criar(double temperatura, DateTime dataRegistro, Cidade cidade)
    {
        return new HistoricoTemperatura(temperatura, dataRegistro, cidade);
    }

    public void AtualizarTemperatura(double novaTemperatura)
    {
        Temperatura = novaTemperatura;
        AtualizadoEm = DateTime.UtcNow;
    }
}