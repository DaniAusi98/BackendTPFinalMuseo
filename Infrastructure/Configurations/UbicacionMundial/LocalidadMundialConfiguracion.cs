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
    public class LocalidadMundialConfiguration
    : IEntityTypeConfiguration<Localidad>
    {
        public void Configure(EntityTypeBuilder<Localidad> builder)
        {
            builder.ToTable("LocalidadesMundial");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Nombre)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Tipo)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.CodigoPostal)
                .HasMaxLength(20);

            builder.Property(x => x.Latitud);

            builder.Property(x => x.Longitud);

            builder.Property(x => x.DivisionAdministrativaId)
                .HasMaxLength(255);

            builder.HasOne(x => x.DivisionAdministrativa)
                .WithMany(x => x.Localidades)
                .HasForeignKey(x => x.DivisionAdministrativaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
