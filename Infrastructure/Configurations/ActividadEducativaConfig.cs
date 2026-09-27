using Domain.ActividadesAreaEducacion.Entities;
using Domain.ActividadMuseo.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configurations
{
    internal sealed class ActividadEducativaConfig : IEntityTypeConfiguration<ActividadEducativa>

    {
        public void Configure(EntityTypeBuilder<ActividadEducativa> builder)
        {
            builder.ToTable("ActividadesEducativas");
            builder.HasBaseType<ActividadMuseo>();

            builder.Property(x => x.Titulo)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(x => x.TipoActividadEducativa)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();                
            builder.Property(x => x.ProyectoVinculadaId)
                .IsRequired()
                .HasMaxLength(100);
            builder.HasOne(x => x.ProyectoVinculada)
                .WithMany()
                .HasForeignKey(x => x.ProyectoVinculadaId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Property(x => x.InstitucionVinculada)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(x => x.PublicoObjetivo)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(x => x.Observaciones)
                .HasMaxLength(1000);

            builder.Property(x => x.RequiereDifusion)
                .IsRequired();
            builder.Property(x => x.SolicitaFlyer)
                .IsRequired();
            builder.Property(x => x.UrlImagenes)
               .HasConversion(
                   v => System.Text.Json.JsonSerializer.Serialize(
                       v,
                       (System.Text.Json.JsonSerializerOptions?)null),

                   v => string.IsNullOrEmpty(v)
                       ? new List<string>()
                       : System.Text.Json.JsonSerializer.Deserialize<List<string>>(
                           v,
                           (System.Text.Json.JsonSerializerOptions?)null)!)
               .HasColumnType("json")
               .IsRequired(false);
        }
    }
}
