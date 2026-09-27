using Domain.ActividadMuseo.Entities;
using Domain.VisitasGrupales.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.VisitasGrupales
{
    public class VisitaGrupalAutoguiadaConfiguration
        : IEntityTypeConfiguration<VisitaGrupalAutoguiada>
    {
        public void Configure(EntityTypeBuilder<VisitaGrupalAutoguiada> builder)
        {
            builder.ToTable("VisitasGrupalesAutoguiadas");

            builder.HasBaseType<ActividadMuseo>();

            builder.Property(x => x.UsuarioVisitanteId)
                .IsRequired();

            builder.Property(x => x.Institucion)
                .HasMaxLength(200)
                .IsRequired();


            builder.Property(x => x.PaisInstitucion)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.ProvinciaInstitucion)
                .HasMaxLength(100)
                .IsRequired();


            builder.Property(x => x.LocalidadInstitucion)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.DiversidadFuncional)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(x => x.Observaciones)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(x => x.EstadoConfirmacion)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();


            builder.OwnsOne(x => x.EmailInstitucion, email =>
            {
                email.Property(e => e.Valor)
                    .HasColumnName("EmailInstitucion")
                    .HasMaxLength(200)
                    .IsRequired();
            });

            builder.HasMany(x => x.Tematicas)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "VisitaGrupalAutoguiadaTematicas",
                    j => j
                        .HasOne<TematicaVisita>()
                        .WithMany()
                        .HasForeignKey("TematicaId"),

                    j => j
                        .HasOne<VisitaGrupalAutoguiada>()
                        .WithMany()
                        .HasForeignKey("VisitaGrupalAutoguiadaId"),

                    j =>
                    {
                        j.HasKey(
                            "VisitaGrupalAutoguiadaId",
                            "TematicaId"
                        );
                    }
                );
        }
    }
}
