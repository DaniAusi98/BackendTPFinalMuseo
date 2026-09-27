using Domain.RecursoMuseo.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class SalaConfiguration : IEntityTypeConfiguration<Sala>
    {
        public void Configure(EntityTypeBuilder<Sala> builder)
        {
            // 1. Nombre de la tabla y clave primaria (heredada de DomainEntity)
            builder.ToTable("Salas");
            builder.HasKey(x => x.Id);

            // 2. Configuración de textos obligatorios
            builder.Property(x => x.Nombre)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.CodigoSala)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(x => x.EstadoSala)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            // 3. Configuración de Enums (los guardamos como texto en la BD)
            builder.Property(x => x.TipoSala)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Ubicacion)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            // 4. Propiedades numéricas
            builder.Property(x => x.Capacidad)
                .IsRequired();
        }
    }
}
