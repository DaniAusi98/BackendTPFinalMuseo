using Domain.RecursoMuseo.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ConfiguracionSalaActividadConfig : IEntityTypeConfiguration<ConfiguracionSalaActividad>
    {
        public void Configure(EntityTypeBuilder<ConfiguracionSalaActividad> builder)
        {
            builder.ToTable("ConfiguracionesSalaActividad");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.SalaId)
                .IsRequired()
                .HasMaxLength(36);

            builder.Property(x => x.TipoActividad)
                .IsRequired();

            builder.Property(x => x.Habilitada)
                .IsRequired();

            builder.Property(x => x.CapacidadMaxima)
                .IsRequired(false);

            // Índice para búsquedas frecuentes
            builder.HasIndex(x => x.SalaId);
            builder.HasIndex(x => x.TipoActividad);
            builder.HasIndex(x => new { x.SalaId, x.TipoActividad })
                .IsUnique();
        }
    }
}
