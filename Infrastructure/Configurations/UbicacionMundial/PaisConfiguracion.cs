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
    public class PaisConfiguration : IEntityTypeConfiguration<Pais>
    {
        public void Configure(EntityTypeBuilder<Pais> builder)
        {
            builder.ToTable("Paises");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Nombre)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Codigo)
                .HasMaxLength(10)
                .IsRequired();

            builder.HasMany(x => x.Divisiones)
                .WithOne(x => x.Pais)
                .HasForeignKey(x => x.PaisId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
