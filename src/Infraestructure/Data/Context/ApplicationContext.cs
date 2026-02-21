using Domain.Entities.Cidades;
using Domain.Entities.HistoricosTemperaturas;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Data.Context;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }

    public DbSet<Cidade> Cidades { get; set; }

    public DbSet<HistoricoTemperatura> HistoricosTemperaturas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
    }
}