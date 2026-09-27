using Domain.VisitasGrupales.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

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

            builder.OwnsOne(
                c => c.HorarioDisponibleVisitaAutoguiadas,
                horario =>
                {
                    horario.Property(h => h.HoraInicio)
                        .HasColumnName("HoraInicio")
                        .IsRequired();

                    horario.Property(h => h.HoraFin)
                        .HasColumnName("HoraFin")
                        .IsRequired();
                });

            builder.OwnsOne(
                c => c.DiasDisponibles,
                dias =>
                {
                    dias.Property(d => d.Dias)
                        .HasColumnName("DiasDisponibles")
                        .HasConversion(
                            v => JsonSerializer.Serialize(
                                v,
                                (JsonSerializerOptions)null),
                            v => JsonSerializer.Deserialize<List<DayOfWeek>>(
                                v,
                                (JsonSerializerOptions)null)
                                ?? new List<DayOfWeek>())
                        .IsRequired();
                });

            builder.OwnsMany(
                c => c.Bloqueos,
                bloqueo =>
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