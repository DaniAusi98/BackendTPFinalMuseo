using Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal sealed class ConfiguracionCalendarioMuseo
        : IEntityTypeConfiguration<HorarioAperturaMuseo>
    {
        public void Configure(EntityTypeBuilder<HorarioAperturaMuseo> builder)
        {
            builder.ToTable("CalendariosMuseo");

            builder.HasKey(x => x.Id);

            // Horarios de apertura por día de la semana.
            builder.OwnsMany(x => x.HorariosApertura, horario =>
            {
                horario.ToTable("HorariosAperturaMuseo");

                horario.WithOwner()
                    .HasForeignKey("CalendarioMuseoId");

                horario.Property<int>("Id");
                horario.HasKey("Id");

                horario.Property(h => h.Dia)
                    .IsRequired();

                horario.Property(h => h.HoraInicio)
                    .IsRequired();

                horario.Property(h => h.HoraFin)
                    .IsRequired();
            });

            // Cierres especiales del museo.
            builder.HasMany(x => x.DiasCierre)
                .WithOne()
                .HasForeignKey("CalendarioMuseoId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(x => x.HorariosApertura)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}