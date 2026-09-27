using Domain.VisitasGrupales.Entities.GrupalGuiada;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace Infrastructure.Configurations.VisitasGrupales
{
    internal sealed class ConfiguracionVisitasGrupalesGuiadaConfig : IEntityTypeConfiguration<ConfiguracionVisitasGrupalesGuiadas>
    {
        public void Configure(EntityTypeBuilder<ConfiguracionVisitasGrupalesGuiadas> builder)
        {
            builder.ToTable("ConfiguracionVisitasGrupalesGuiadas");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.MaximoVisitasSimultaneas)                
                .IsRequired();

            builder.Property(c => c.CapacidadPorGuia)
                .IsRequired();

            builder.Property(c => c.CapacidadMaximaPorTurno)
                .IsRequired();

            // Configurar DiasDisponibles como owned type
            builder.OwnsOne(c => c.DiasDisponibles, diasNav =>
            {
                diasNav.Property(d => d.Dias)
                    .HasColumnName("DiasDisponibles")
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<List<DayOfWeek>>(v, (JsonSerializerOptions)null) ?? new List<DayOfWeek>()
                    )
                    .IsRequired();
            });

            // Configurar Turnos como owned collection
            builder.OwnsMany(c => c.Turnos, turnosNav =>
            {
                turnosNav.ToTable("TurnosVisitasGuiadas");

                turnosNav.WithOwner()
                    .HasForeignKey("ConfiguracionVisitasGrupalesGuiadasId");

                turnosNav.Property<int>("Id");
                turnosNav.HasKey("Id");

                turnosNav.Property(t => t.HoraInicio)
                    .HasColumnName("HoraInicio")
                    .IsRequired();

                turnosNav.Property(t => t.HoraFin)
                    .HasColumnName("HoraFin")
                    .IsRequired();
            });
            builder.OwnsMany(c => c.Bloqueos, bloqueosNav =>
            {
                bloqueosNav.ToTable("BloqueosVisitasGuiadas");

                bloqueosNav.WithOwner()
                    .HasForeignKey("ConfiguracionVisitasGrupalesGuiadasId");

                bloqueosNav.Property<int>("Id");
                bloqueosNav.HasKey("Id");

                bloqueosNav.Property(b => b.FechaDesde)
                    .IsRequired();

                bloqueosNav.Property(b => b.FechaHasta)
                    .IsRequired();

                bloqueosNav.Property(b => b.Motivo)
                    .HasMaxLength(500)
                    .IsRequired();
            });
        }
    }
}
