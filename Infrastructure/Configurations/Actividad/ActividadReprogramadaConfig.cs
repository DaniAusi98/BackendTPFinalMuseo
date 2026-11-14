using Domain.ActividadMuseo.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Actividad
{
    public class ActividadReprogramadaConfiguration : IEntityTypeConfiguration<ActividadReprogramada>
    {
        public void Configure(EntityTypeBuilder<ActividadReprogramada> builder)
        {
            builder.ToTable("ActividadesReprogramadas");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ActividadId)
                .IsRequired();

            builder.HasOne(x => x.Actividad)
                .WithMany()
                .HasForeignKey(x => x.ActividadId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // HORARIO ANTERIOR
            // =========================

            builder.OwnsOne(x => x.HorarioAnterior, h =>
            {
                h.ToTable("ReprogramacionHorarioAnterior");

                h.WithOwner()
                    .HasForeignKey("ActividadReprogramadaId");

                h.Property<int>("Id");

                h.HasKey("Id");

                h.Property(x => x.Inicio)
                    .HasColumnType("datetime");

                h.Property(x => x.Fin)
                    .HasColumnType("datetime");
            });

            // =========================
            // HORARIO NUEVO
            // =========================

            builder.OwnsOne(x => x.HorarioNuevo, h =>
            {
                h.ToTable("ReprogramacionHorarioNuevo");

                h.WithOwner()
                    .HasForeignKey("ActividadReprogramadaId");

                h.Property<int>("Id");

                h.HasKey("Id");

                h.Property(x => x.Inicio)
                    .HasColumnType("datetime");

                h.Property(x => x.Fin)
                    .HasColumnType("datetime");
            });

            builder.Property(x => x.FechaReprogramacion)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(x => x.Motivo)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(x => x.UsuarioId)
                .IsRequired(false);

            builder.HasIndex(x => x.ActividadId);
        }
    }
}