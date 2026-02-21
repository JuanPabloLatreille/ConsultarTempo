using Domain.Interfaces.Services;
using Infraestructure.Services.ProvedorClima;
using Infraestructure.Services.ProvedorClima.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infraestructure.DependencyInjections;

public static class ServicesInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OpenWeatherMapOptions>(
            configuration.GetSection(OpenWeatherMapOptions.SectionName));

        services.AddScoped<IProvedorClimaService, ProvedorClimaService>();

        return services;
    }
}