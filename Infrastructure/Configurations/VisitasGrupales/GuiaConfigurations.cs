using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Domain.RecursoMuseo.Entities.Guia;

namespace Infrastructure.Configurations.VisitasGrupales
{
    public class GuiaConfiguration : IEntityTypeConfiguration<Guia>
    {
        public void Configure(EntityTypeBuilder<Guia> builder)
        {
            builder.ToTable("Guias");

            builder.HasKey(g => g.Id);
            builder.Property(g => g.NombreCompleto)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(g => g.Activo)
                .IsRequired();

            builder.Property(g => g.PersonalInternoId)
                .IsRequired();

            // HorariosGuia como entidad separada (HasMany)
            builder.HasMany(g => g.HorariosGuia)
                   .WithOne(h => h.Guia)
                   .HasForeignKey(h => h.GuiaId)
                   .OnDelete(DeleteBehavior.Cascade);

            // AusenciasProgramadas
            builder.HasMany(g => g.AusenciasProgramadas)
                .WithOne(a => a.Guia)
                .HasForeignKey(a => a.GuiaId)
                .OnDelete(DeleteBehavior.Cascade);


        }
    }
}

