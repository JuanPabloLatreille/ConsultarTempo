using Domain.Interfaces;
using Domain.Interfaces.Cidades;
using Domain.Interfaces.HistoricosTemperaturas;
using Domain.Interfaces.Usuarios;
using Infraestructure.Repositories;
using Infraestructure.Repositories.Cidades;
using Infraestructure.Repositories.HistoricosTemperaturas;
using Infraestructure.Repositories.Usuarios;
using Microsoft.Extensions.DependencyInjection;

namespace Infraestructure.DependencyInjections;

public static class RepositoryInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICidadeRepository, CidadeRepository>();
        services.AddScoped<IHistoricoTemperaturaRepository, HistoricoTemperaturaRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();

        return services;
    }
}