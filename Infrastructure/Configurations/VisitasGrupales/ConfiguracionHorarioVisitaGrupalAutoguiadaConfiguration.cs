using Domain.VisitasGrupales.Entities.GrupalAutoguiada;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.VisitasGrupales
{
    internal sealed class ConfiguracionHorarioVisitaGrupalAutoguiadaConfiguration
        : IEntityTypeConfiguration<ConfiguracionHorarioAutoguiada>
    {
        public void Configure(
            EntityTypeBuilder<ConfiguracionHorarioAutoguiada> builder)
        {
            builder.ToTable("ConfiguracionVisitaAutoguiada");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.CapacidadMaximaPorGrupo)
                .IsRequired();

            builder.Property(c => c.VisitasSimultaneasMaximas)
                .IsRequired();

            builder.Property(c => c.DuracionVisita)
                .IsRequired()
                .HasConversion(
                    v => v.Ticks,
                    v => TimeSpan.FromTicks(v));

            builder.Property(c => c.IntervaloReservas)
                .IsRequired()
                .HasConversion(
                    v => v.Ticks,
                    v => TimeSpan.FromTicks(v));

            // Horarios disponibles: un registro por value object.
            builder.OwnsMany(c => c.DiasDisponibles, horario =>
            {
                horario.ToTable("HorariosDisponiblesAutoguiadas");

                horario.WithOwner()
                    .HasForeignKey("ConfiguracionHorarioAutoguiadaId");

                horario.Property<int>("Id");
                horario.HasKey("Id");

                horario.Property(h => h.Dia)
                    .IsRequired();

                horario.Property(h => h.Desde)
                    .HasColumnName("Desde")
                    .IsRequired();

                horario.Property(h => h.Hasta)
                    .HasColumnName("Hasta")
                    .IsRequired();
            });

            builder.OwnsMany(c => c.Bloqueos, bloqueo =>
            {
                bloqueo.ToTable("BloqueosVisitasAutoguiadas");

                bloqueo.WithOwner()
                    .HasForeignKey("ConfiguracionHorarioAutoguiadaId");

                bloqueo.Property<int>("Id");
                bloqueo.HasKey("Id");

                bloqueo.Property(b => b.FechaDesde)
                    .HasColumnName("FechaDesde")
                    .IsRequired();

                bloqueo.Property(b => b.FechaHasta)
                    .HasColumnName("FechaHasta")
                    .IsRequired();

                bloqueo.Property(b => b.Motivo)
                    .HasColumnName("Motivo")
                    .HasMaxLength(500)
                    .IsRequired();
            });
        }
    }
}