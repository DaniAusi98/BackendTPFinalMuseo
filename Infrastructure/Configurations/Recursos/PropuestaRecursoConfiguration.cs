using Domain.Eventos.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Recursos
{
    public class PropuestaRecursoConfiguration : IEntityTypeConfiguration<PropuestasRecursos>
    {
        public void Configure(EntityTypeBuilder<PropuestasRecursos> builder)
        {
            builder.ToTable("PropuestaRecursosReservados");

            // Configura la clave primaria que hereda de DomainEntity
            builder.HasKey(x => x.Id);

            // Mapea la relación hacia el Recurso (el cual no sabe nada de esta clase)
            builder.HasOne(x => x.Recurso)
                .WithMany() // Vacío porque Recurso no tiene una lista de RecursosAsignados
                .HasForeignKey(x => x.RecursoId)
                .OnDelete(DeleteBehavior.Restrict); // Evita borrar un Recurso base si está asignado a una actividad

            builder.HasIndex(x => new { x.PropuestaId, x.RecursoId })
                .IsUnique();

            builder.Property(x => x.CantidadAsignada)
                .IsRequired();


        }
    }
}