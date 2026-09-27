using Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class DepartamentoConfiguration : IEntityTypeConfiguration<Departamento>
    {
        public void Configure(EntityTypeBuilder<Departamento> builder)
        {
            builder.ToTable("Departamentos");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Nombre)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(d => d.ProvinciaId)
                .IsRequired();

            // FK to Provincia (no navigation property required)
            builder.HasOne<Provincia>()
                .WithMany()
                .HasForeignKey(d => d.ProvinciaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(d => new { d.ProvinciaId, d.Nombre }).IsUnique(false);
        }
    }
}
