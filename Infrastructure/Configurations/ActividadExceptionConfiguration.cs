using Domain.ActividadMuseo.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ActividadExceptionConfiguration : IEntityTypeConfiguration<ActividadException>
    {
        public void Configure(EntityTypeBuilder<ActividadException> builder)
        {
            builder.ToTable("ActividadExceptions");

            // Clave primaria heredada de DomainEntity<string>
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasMaxLength(50); // Ajustado al tamaño de un Guid en string

            // Mapeo explícito de la propiedad de clave foránea física
            builder.Property(x => x.ActividadMuseoId)
                .HasColumnName("ActividadMuseoId")
                .HasMaxLength(50) // Debe coincidir con el tipo/tamaño del Id de ActividadMuseo
                .IsRequired();

            // Mapeo del campo FechaExcluir (tipo DateTime)
            builder.Property(x => x.FechaExcluir)
                .HasColumnName("FechaExcluir")
                .HasColumnType("datetime")
                .IsRequired();

            // Mapeo del motivo opcional de la cancelación
            builder.Property(x => x.Motivo)
                .HasColumnName("Motivo")
                .HasMaxLength(255)
                .IsRequired(false);

            // ============================================================
            // INDEXACIÓN COMPUESTA PARA RENDIMIENTO
            // ============================================================
            // Creamos el índice compuesto sobre las propiedades físicas
            // para que las búsquedas en memoria del Factory vuelen.
            builder.HasIndex(x => new { x.ActividadMuseoId, x.FechaExcluir })
                .HasDatabaseName("IX_ActividadExceptions_ActividadId_Fecha");
        }
    }
}
