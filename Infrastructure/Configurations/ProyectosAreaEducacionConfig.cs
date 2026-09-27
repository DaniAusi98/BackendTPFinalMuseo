using Domain.ActividadesAreaEducacion.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal sealed class ProyectosAreaEducacionConfig
        : IEntityTypeConfiguration<ProyectosAreaEducacion>
    {
        public void Configure(EntityTypeBuilder<ProyectosAreaEducacion> builder)
        {
            builder.ToTable("ProyectosAreaEducacion");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Nombre)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.FechaInicio)
                .IsRequired();

            builder.Property(x => x.FechaFin)
                .IsRequired();

            builder.Property(x => x.Publico)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasMany(x => x.EquipoTrabajoResponsable)
                .WithOne(x => x.Proyecto)
                .HasForeignKey(x => x.ProyectoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}