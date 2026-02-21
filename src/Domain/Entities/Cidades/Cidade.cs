using Domain.Entities.HistoricosTemperaturas;

namespace Domain.Entities.Cidades;

public sealed class Cidade : EntidadeBase
{
    private readonly List<HistoricoTemperatura> _historicosTemperaturas = [];

    private Cidade()
    {
        // Construtor privado para uso do Entity Framework
    }

    private Cidade(string nome, string uf, double latitude, double longitude)
    {
        Nome = nome;
        Uf = uf;
        Latitude = latitude;
        Longitude = longitude;
    }

    public string Nome { get; private set; }

    public string Uf { get; private set; }

    public double Latitude { get; private set; }

    public double Longitude { get; private set; }

    public IReadOnlyCollection<HistoricoTemperatura> HistoricosTemperaturas => _historicosTemperaturas.AsReadOnly();

    public static Cidade Criar(string nome, string uf, double latitude, double longitude)
    {
        return new Cidade(nome, uf, latitude, longitude);
    }

    public void AdicionarHistoricoTemperatura(HistoricoTemperatura historico)
    {
        _historicosTemperaturas.Add(historico);
        AtualizadoEm = DateTime.UtcNow;
    }
}