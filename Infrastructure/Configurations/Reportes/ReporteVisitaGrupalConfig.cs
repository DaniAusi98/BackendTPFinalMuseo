using Domain.Reportes.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Reportes
{
    internal class ReporteVisitaGrupalConfig : IEntityTypeConfiguration<ReporteGeneralVisitasGrupales>
    {
        public void Configure(EntityTypeBuilder<ReporteGeneralVisitasGrupales> builder)
        {
            builder.ToTable("ReporteVisitasGrupales");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ReservasTotales).IsRequired();
            builder.Property(x => x.VisitanteTotales).IsRequired();
            builder.Property(x => x.VisitasConfirmadas).IsRequired();
            builder.Property(x => x.VisitasCanceladas).IsRequired();
            builder.Property(x => x.Reprogramadas).IsRequired();
            builder.Property(x => x.Pendientes).IsRequired();

        }
    }
}
