using Domain.RecursoMuseo.Entities.Guia;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Configurations.VisitasGrupales
{
    public class HorarioGuiaConfiguration
    : IEntityTypeConfiguration<HorarioGuia>
    {
        public void Configure(EntityTypeBuilder<HorarioGuia> builder)
        {
            builder.ToTable("HorariosGuia");

            builder.HasKey(h => h.Id);

            builder.Property(h => h.HoraInicio)
                .IsRequired();

            builder.Property(h => h.HoraFin)
                .IsRequired();


            builder.OwnsOne(h => h.DiaAsignado, dia =>
            {
                dia.Property(d => d.Dia)
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .IsRequired();
            });
        }
    }
}
