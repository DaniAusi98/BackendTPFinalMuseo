using Domain.VisitasGrupales.Entities.GrupalGuiada;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.VisitasGrupales
{
    internal sealed class ConfiguracionVisitasGrupalesGuiadaConfig
        : IEntityTypeConfiguration<ConfiguracionVisitasGrupalesGuiadas>
    {
        public void Configure(
            EntityTypeBuilder<ConfiguracionVisitasGrupalesGuiadas> builder)
        {
            builder.ToTable("ConfiguracionVisitasGrupalesGuiadas");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.MaximoVisitasSimultaneas)
                .IsRequired();

            builder.Property(c => c.CapacidadPorGuia)
                .IsRequired();

            builder.Property(c => c.CapacidadMaximaPorTurno)
                .IsRequired();

            builder.OwnsMany(c => c.DiasDisponibles, horarios =>
            {
                horarios.ToTable("HorariosDisponiblesVisitasGuiadas");

                horarios.WithOwner()
                    .HasForeignKey("ConfiguracionVisitasGrupalesGuiadasId");

                horarios.Property<int>("Id");
                horarios.HasKey("Id");

                horarios.Property(h => h.Dia)
                    .IsRequired();

                horarios.Property(h => h.Desde)
                    .HasColumnName("Desde")
                    .IsRequired();

                horarios.Property(h => h.Hasta)
                    .HasColumnName("Hasta")
                    .IsRequired();
            });

            builder.Navigation(c => c.DiasDisponibles)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.OwnsMany(c => c.Bloqueos, bloqueos =>
            {
                bloqueos.ToTable("BloqueosVisitasGuiadas");

                bloqueos.WithOwner()
                    .HasForeignKey("ConfiguracionVisitasGrupalesGuiadasId");

                bloqueos.Property<int>("Id");
                bloqueos.HasKey("Id");

                bloqueos.Property(b => b.FechaDesde)
                    .IsRequired();

                bloqueos.Property(b => b.FechaHasta)
                    .IsRequired();

                bloqueos.Property(b => b.Motivo)
                    .HasMaxLength(500)
                    .IsRequired();
            });
        }
    }
}