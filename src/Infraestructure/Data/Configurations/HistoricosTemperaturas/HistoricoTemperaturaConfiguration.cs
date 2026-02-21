using Domain.Entities.HistoricosTemperaturas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Data.Configurations.HistoricosTemperaturas;

public class HistoricoTemperaturaConfiguration : IEntityTypeConfiguration<HistoricoTemperatura>
{
    public void Configure(EntityTypeBuilder<HistoricoTemperatura> builder)
    {
        builder.ToTable("historicos_temperaturas");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(h => h.Temperatura)
            .HasColumnName("temperatura")
            .IsRequired();

        builder.Property(h => h.DataRegistro)
            .HasColumnName("data_registro")
            .IsRequired();

        builder.Property(h => h.CidadeId)
            .HasColumnName("cidade_id")
            .IsRequired();

        builder.Property(h => h.CriadoEm)
            .HasColumnName("criado_em")
            .IsRequired();

        builder.Property(h => h.AtualizadoEm)
            .HasColumnName("atualizado_em");

        builder.Property(h => h.Deletado)
            .HasColumnName("deletado")
            .HasDefaultValue(false)
            .IsRequired();

        builder.HasOne(h => h.Cidade)
            .WithMany(c => c.HistoricosTemperaturas)
            .HasForeignKey(h => h.CidadeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(h => !h.Deletado);

        builder.HasIndex(h => h.CidadeId)
            .HasDatabaseName("ix_historicos_temperaturas_cidade_id");

        builder.HasIndex(h => h.DataRegistro)
            .HasDatabaseName("ix_historicos_temperaturas_data_registro");
    }
}