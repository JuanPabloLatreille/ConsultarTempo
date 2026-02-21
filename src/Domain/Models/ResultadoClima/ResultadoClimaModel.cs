namespace Domain.Models.ResultadoClima;

public sealed record ResultadoClimaModel(
    string NomeCidade,
    string CodigoPais,
    double Temperatura,
    double Latitude,
    double Longitude);