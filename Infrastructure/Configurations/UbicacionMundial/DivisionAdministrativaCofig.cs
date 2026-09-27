using Domain.Common.Entities.Ubicacion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configurations.UbicacionMundial
{
    public class DivisionAdministrativaConfiguration
      : IEntityTypeConfiguration<DivisionAdministrativa>
    {
        public void Configure(EntityTypeBuilder<DivisionAdministrativa> builder)
        {
            builder.ToTable("DivisionesAdministrativas");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Nombre)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Tipo)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Nivel)
                .IsRequired();

            builder.Property(x => x.PaisId)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.PadreId)
                .HasMaxLength(255);

            // País -> Divisiones
            builder.HasOne(x => x.Pais)
                .WithMany(x => x.Divisiones)
                .HasForeignKey(x => x.PaisId)
                .OnDelete(DeleteBehavior.Cascade);

            // División -> División padre
            builder.HasOne(x => x.Padre)
                .WithMany(x => x.Hijas)
                .HasForeignKey(x => x.PadreId)
                .OnDelete(DeleteBehavior.Restrict);

            // División -> Localidades
            builder.HasMany(x => x.Localidades)
                .WithOne(x => x.DivisionAdministrativa)
                .HasForeignKey(x => x.DivisionAdministrativaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
