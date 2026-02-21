using Domain.Interfaces.Services;
using Infraestructure.Services.ProvedorClima;
using Infraestructure.Services.ProvedorClima.Options;
using Infraestructure.Services.CriptografarSenha;
using Infraestructure.Services.GerarTokenJwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infraestructure.DependencyInjections;

public static class ServicesInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OpenWeatherMapOptions>(
            configuration.GetSection(OpenWeatherMapOptions.SectionName));
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

        services.AddScoped<IProvedorClimaService, ProvedorClimaService>();
        services.AddScoped<ICriptografarSenhaService, CriptografarSenhaService>();
        services.AddScoped<IGerarTokenJwtService, GerarTokenJwtService>();

        return services;
    }
}