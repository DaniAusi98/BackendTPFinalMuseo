
using Domain.ActividadMuseo.Entities;
using Domain.VisitasGrupales.Entities.GrupalGuiada;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class VisitaReprogramadaConfig
        : IEntityTypeConfiguration<VisitaReprogramada>
    {
        public void Configure(EntityTypeBuilder<VisitaReprogramada> builder)
        {
            builder.ToTable("VisitasReprogramadas");

            builder.HasKey(x => x.Id);

            // =========================
            // IDS Y CLAVES FORÁNEAS
            // =========================

            builder.Property(x => x.VisitaReprogramadaId)
                .IsRequired();

            builder.Property(x => x.NuevaVisitaId)
                .IsRequired();

            builder.HasOne<ActividadMuseo>()
                .WithMany()
                .HasForeignKey(x => x.VisitaReprogramadaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ActividadMuseo>()
                .WithMany()
                .HasForeignKey(x => x.NuevaVisitaId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // DATOS DE LA INSTITUCIÓN
            // =========================

            builder.Property(x => x.Institucion)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.NivelEducativo)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(x => x.AnioGrado)
                .HasMaxLength(50)
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

            // =========================
            // ESTADO Y CANTIDAD
            // =========================

            builder.Property(x => x.Estado)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.CantidadPersonas)
                .IsRequired(false);

            // =========================
            // HORARIO ANTERIOR
            // =========================

            builder.OwnsOne(x => x.HorarioReprogrado, horario =>
            {
                horario.Property(x => x.Inicio)
                    .HasColumnName("HorarioAnteriorInicio")
                    .HasColumnType("datetime")
                    .IsRequired();

                horario.Property(x => x.Fin)
                    .HasColumnName("HorarioAnteriorFin")
                    .HasColumnType("datetime")
                    .IsRequired();
            });

            builder.Navigation(x => x.HorarioReprogrado)
                .IsRequired();

            // =========================
            // HORARIO NUEVO
            // =========================

            builder.OwnsOne(x => x.NuevoHorario, horario =>
            {
                horario.Property(x => x.Inicio)
                    .HasColumnName("NuevoHorarioInicio")
                    .HasColumnType("datetime")
                    .IsRequired();

                horario.Property(x => x.Fin)
                    .HasColumnName("NuevoHorarioFin")
                    .HasColumnType("datetime")
                    .IsRequired();
            });

            builder.Navigation(x => x.NuevoHorario)
                .IsRequired();

            // =========================
            // ÍNDICES
            // =========================

            builder.HasIndex(x => x.VisitaReprogramadaId);
            builder.HasIndex(x => x.NuevaVisitaId);
        }
    }
}

