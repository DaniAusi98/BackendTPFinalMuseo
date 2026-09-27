using Domain.ActividadMuseo.Entities;
using Domain.VisitasGrupales.Entities;
using Domain.VisitasGrupales.Entities.GrupalGuiada;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.VisitasGrupales
{
    public class VisitaGrupalGuiadaConfiguration
        : IEntityTypeConfiguration<VisitaGrupalGuiada>
    {
        public void Configure(EntityTypeBuilder<VisitaGrupalGuiada> builder)
        {
            builder.ToTable("VisitasGrupalesGuiadas");

            // TPT
            builder.HasBaseType<ActividadMuseo>();


            builder.Property(x => x.UsuarioVisitanteId)
                .IsRequired();


            builder.Property(x => x.Institucion)
                .HasMaxLength(200)
                .IsRequired();


            builder.Property(x => x.NivelEducativo)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired(false);


            builder.Property(x => x.AnioGrado)
                .IsRequired(false);

            builder.Property(x => x.PaisInstitucion)
           .HasMaxLength(100)
           .IsRequired();

            builder.Property(x => x.ProvinciaInstitucion)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.LocalidadInstitucion)
                .HasMaxLength(100)
                .IsRequired();


            builder.Property(x => x.DiversidadFuncionalDescripcion)
                .HasMaxLength(500)
                .IsRequired(false);


            builder.Property(x => x.MotivoRelacionVisita)
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


            builder.OwnsOne(x => x.TelefonoInstitucion, telefono =>
            {
                telefono.Property(t => t.Valor)
                    .HasColumnName("TelefonoInstitucion")
                    .HasMaxLength(50)
                    .IsRequired();
            });


            builder.HasMany(x => x.Tematicas)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "VisitaGuiadaTematicas",
                    j => j
                        .HasOne<TematicaVisita>()
                        .WithMany()
                        .HasForeignKey("TematicaId"),

                    j => j
                        .HasOne<VisitaGrupalGuiada>()
                        .WithMany()
                        .HasForeignKey("VisitaGrupalGuiadaId"),

                    j =>
                    {
                        j.HasKey(
                            "VisitaGrupalGuiadaId",
                            "TematicaId"
                        );
                    }
                );
        }
    }
}
