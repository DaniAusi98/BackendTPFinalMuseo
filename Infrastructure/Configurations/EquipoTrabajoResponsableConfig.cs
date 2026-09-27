using Domain.ActividadesAreaEducacion.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal sealed class EquipoTrabajoResponsableConfig
        : IEntityTypeConfiguration<EquipoTrabajoResponsable>
    {
        public void Configure(EntityTypeBuilder<EquipoTrabajoResponsable> builder)
        {
            builder.ToTable("EquiposTrabajoResponsables");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.NombreCompleto)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.InstitucionPerteneciente)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.ProyectoId)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}