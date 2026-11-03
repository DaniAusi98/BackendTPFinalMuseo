using Domain.Eventos.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.EventoConfiguraciones
{
    public class PropuestaMuestraConfigurations : IEntityTypeConfiguration<PropuestaMuestra>
    {
        public void Configure(EntityTypeBuilder<PropuestaMuestra> builder)
        {
            builder.ToTable("PropuestaMuestra");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.MuestraTemporal)
                .WithOne()
                .HasForeignKey<PropuestaMuestra>(p => p.MuestraTemporalId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(x => x.FormularioCompletado)
                .IsRequired();

            builder.Property(x => x.FormularioEnviado)
                .IsRequired();

            builder.Property(x => x.FechaEnvioFormulario)
                .IsRequired(false);

        }
    }
}
/* public class PropuestaEventoConfiguracion
        : IEntityTypeConfiguration<PropuestaEvento>
    {
        public void Configure(EntityTypeBuilder<PropuestaEvento> builder)
        {
            builder.ToTable("PropuestaEvento");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Evento)
                .WithOne()
                .HasForeignKey<PropuestaEvento>(x => x.EventoId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.FormularioEventoCompletado)
                .IsRequired();

            builder.Property(x => x.FormularioEnviado)
                .IsRequired();

            builder.Property(x => x.FechaEnvioFormulario)
                .IsRequired(false);
        }
    }*/