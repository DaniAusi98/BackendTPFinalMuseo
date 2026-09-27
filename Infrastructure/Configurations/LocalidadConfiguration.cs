using Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class LocalidadConfiguration : IEntityTypeConfiguration<LocalidadArg>
    {
        public void Configure(EntityTypeBuilder<LocalidadArg> builder)
        {
            builder.ToTable("Localidades");

            builder.HasKey(l => l.Id);

            builder.Property(l => l.Nombre)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(l => l.ProvinciaId)
                .IsRequired();

            builder.Property(l => l.DepartamentoId)
                .IsRequired();

            // FK to Provincia
            builder.HasOne<Provincia>()
                .WithMany()
                .HasForeignKey(l => l.ProvinciaId)
                .OnDelete(DeleteBehavior.Restrict);

            // FK to Departamento
            builder.HasOne<Departamento>()
                .WithMany()
                .HasForeignKey(l => l.DepartamentoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(l => new { l.ProvinciaId, l.DepartamentoId, l.Nombre }).IsUnique(false);
        }
    }
}
