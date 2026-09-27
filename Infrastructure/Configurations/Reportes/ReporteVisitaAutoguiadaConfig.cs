using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Reportes.Entities;

namespace Infrastructure.Configurations.Reportes
{
    internal sealed class ReporteVisitaAutoguiadaConfig : IEntityTypeConfiguration<ReporteVisitasAutoguiadas>
    {
        public void Configure(EntityTypeBuilder<ReporteVisitasAutoguiadas> builder)
        {
            builder.ToTable("ReporteVisitaAutoguiada");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ReservasTotales).IsRequired();
            builder.Property(x => x.VisitanteTotales).IsRequired();
            builder.Property(x => x.VisitasConfirmadas).IsRequired();
            builder.Property(x => x.VisitasCanceladas).IsRequired();
            builder.Property(x => x.Reprogramadas).IsRequired();
            builder.Property(x => x.Pendientes).IsRequired();
            builder.Property(x => x.TasaOcupacion).IsRequired().HasColumnType("decimal(5,2)");

        }
    }
}
