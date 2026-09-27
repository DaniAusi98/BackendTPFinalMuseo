using Domain.RecursoMuseo.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class RecursoConfiguration : IEntityTypeConfiguration<Recurso>
    {
        public void Configure(EntityTypeBuilder<Recurso> builder)
        {
            // 1. Nombre de la tabla y clave primaria (heredada de DomainEntity)
            builder.ToTable("Recursos");
            builder.HasKey(x => x.Id);

            // 2. Configuración de textos obligatorios
            builder.Property(x => x.NombreRecurso)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(x => x.Descripcion)
                .HasMaxLength(500)
                .IsRequired();

            // 3. Configuración de Enums (almacenados como texto)
            builder.Property(x => x.TipoRecurso)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Estado)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            // 4. Propiedades numéricas
            builder.Property(x => x.CantidadTotal)
                .IsRequired();
        }
    }
}
