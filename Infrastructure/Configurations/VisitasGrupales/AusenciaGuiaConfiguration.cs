using Domain.RecursoMuseo.Entities.Guia;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.VisitasGrupales
{
    public class AusenciaGuiaConfiguration : IEntityTypeConfiguration<AusenciaGuia>
    {
        public void Configure(EntityTypeBuilder<AusenciaGuia> builder)
        {
            builder.ToTable("AusenciasGuia");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.GuiaId)
                .IsRequired();

            builder.Property(a => a.FechaDesde)
                .IsRequired();
            builder.Property(a => a.FechaHasta)
                .IsRequired();

            builder.Property(a => a.Motivo)
                .HasMaxLength(300)
                .IsRequired();

        }
    }
}
