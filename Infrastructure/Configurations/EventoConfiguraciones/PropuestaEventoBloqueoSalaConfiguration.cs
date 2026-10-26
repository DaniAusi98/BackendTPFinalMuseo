using Domain.Common.Entities;
using Domain.Eventos.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.EventoConfiguraciones
{
    public class PropuestaEventoBloqueoSalaConfiguracion
        : IEntityTypeConfiguration<PropuestaEventoBloqueoSala>
    {
        public void Configure(
            EntityTypeBuilder<PropuestaEventoBloqueoSala> builder)
        {
            builder.ToTable("PropuestaEventoBloqueosSala");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .IsRequired();

            builder.Property(x => x.PropuestaEventoId)
                .IsRequired();

            builder.Property(x => x.BloqueoSalaId)
                .IsRequired();

            // Una propuesta puede tener muchos registros de bloqueo.
            builder.HasOne<PropuestaEvento>()
                .WithMany(x => x.BloqueosSala)
                .HasForeignKey(x => x.PropuestaEventoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cada BloqueoSala puede asociarse como máximo
            // a un registro PropuestaEventoBloqueoSala.
            builder.HasOne<BloqueoSala>()
                .WithOne()
                .HasForeignKey<PropuestaEventoBloqueoSala>(
                    x => x.BloqueoSalaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}