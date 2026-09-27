using Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace Infrastructure.Configurations
{
    internal sealed class ConfiguracionCalendarioMuseo
        : IEntityTypeConfiguration<CalendarioMuseo>
    {
        public void Configure(EntityTypeBuilder<CalendarioMuseo> builder)
        {
            builder.ToTable("CalendariosMuseo");

            builder.HasKey(x => x.Id);


            // Value Object HorarioMuseo
            builder.OwnsOne(x => x.HorarioApertura, horario =>
            {
                horario.Property(h => h.HoraInicio)
                    .HasColumnName("HoraInicio")
                    .IsRequired();

                horario.Property(h => h.HoraFin)
                    .HasColumnName("HoraFin")
                    .IsRequired();
            });


            // Value Object DiasLaboralesMuseo
            builder.OwnsOne(x => x.DiasApertura, dias =>
            {
                dias.Property(d => d.Dias)
                    .HasColumnName("DiasApertura")
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<List<DayOfWeek>>(
                                v,
                                (JsonSerializerOptions)null
                             ) ?? new List<DayOfWeek>()
                    )
                    .HasColumnType("longtext")
                    .IsRequired();
            });


            // Colección de cierres
            builder.HasMany(x => x.DiasCierre)
                 .WithOne()
                 .HasForeignKey("CalendarioMuseoId")
                 .OnDelete(DeleteBehavior.Cascade);
        }
    }
}