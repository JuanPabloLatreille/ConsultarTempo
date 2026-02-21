using Domain.Entities.Cidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Data.Configurations.Cidades;

public class CidadeConfiguration : IEntityTypeConfiguration<Cidade>
{
    public void Configure(EntityTypeBuilder<Cidade> builder)
    {
        builder.ToTable("cidades");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(c => c.Nome)
            .HasColumnName("nome")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.Uf)
            .HasColumnName("uf")
            .HasMaxLength(2)
            .IsRequired();

        builder.Property(c => c.Latitude)
            .HasColumnName("latitude")
            .IsRequired();

        builder.Property(c => c.Longitude)
            .HasColumnName("longitude")
            .IsRequired();

        builder.Property(c => c.CriadoEm)
            .HasColumnName("criado_em")
            .IsRequired();

        builder.Property(c => c.AtualizadoEm)
            .HasColumnName("atualizado_em");

        builder.Property(c => c.Deletado)
            .HasColumnName("deletado")
            .HasDefaultValue(false)
            .IsRequired();

        builder.HasMany(c => c.HistoricosTemperaturas)
            .WithOne(h => h.Cidade)
            .HasForeignKey(h => h.CidadeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(c => !c.Deletado);
    }
}