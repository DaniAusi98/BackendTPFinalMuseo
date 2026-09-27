using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.ActividadMuseo.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class RecursoAsignadoConfiguration : IEntityTypeConfiguration<RecursoAsignado>
    {
        public void Configure(EntityTypeBuilder<RecursoAsignado> builder)
        {
            builder.ToTable("ActividadRecursosAsignados");

            // Configura la clave primaria que hereda de DomainEntity
            builder.HasKey(x => x.Id);

            // Mapea la relación hacia el Recurso (el cual no sabe nada de esta clase)
            builder.HasOne(x => x.Recurso)
                .WithMany() // Vacío porque Recurso no tiene una lista de RecursosAsignados
                .HasForeignKey(x => x.RecursoId)
                .OnDelete(DeleteBehavior.Restrict); // Evita borrar un Recurso base si está asignado a una actividad

            builder.HasIndex(x => new { x.ActividadId, x.RecursoId })
                .IsUnique();

            builder.Property(x => x.CantidadAsignada)
                .IsRequired();

       
        }
    }
}
