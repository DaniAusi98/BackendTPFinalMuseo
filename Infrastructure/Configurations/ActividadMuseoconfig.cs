using Domain.ActividadMuseo.Entities;
using Domain.RecursoMuseo.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ActividadMuseoConfiguration : IEntityTypeConfiguration<ActividadMuseo>
    {
        public void Configure(EntityTypeBuilder<ActividadMuseo> builder)
        {
            builder.ToTable("ActividadesMuseo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CategoriaActividad)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.TipoActividad)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Estado)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.CantidadPersonas)
                .IsRequired(false);

            // ==========================================
            // RECURRENCIA (Mapeo Simple del String RRule)
            // ==========================================
            // Se almacena como un VARCHAR/NVARCHAR común en la base de datos
            builder.Property(x => x.RRule)
                .HasColumnName("RRule")
                .HasMaxLength(255)
                .IsRequired(false); // Es opcional porque hay actividades únicas
            // =========================
            // HORARIO
            // =========================

            builder.OwnsOne(x => x.Horario, h =>
            {
                h.ToTable("ActividadTimeSlots");

                h.WithOwner()
                    .HasForeignKey("ActividadMuseoId");

                h.Property<int>("Id");

                h.HasKey("Id");

                h.Property(x => x.Inicio)
                    .HasColumnType("datetime");

                h.Property(x => x.Fin)
                    .HasColumnType("datetime");
            });

            // ==========================================
            // EXCEPCIONES (Mapeo de Colección Privada Encapsulada)
            // ==========================================
            builder.HasMany(x => x.Exceptions)
                .WithOne()
                .HasForeignKey("ActividadMuseoId") // Clave foránea en la tabla ActividadExceptions
                .OnDelete(DeleteBehavior.Cascade);

          


            // =========================
            // SALAS
            // =========================

            builder.HasMany(x => x.Salas)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "ActividadSala",
                    j => j
                        .HasOne<Sala>()
                        .WithMany()
                        .HasForeignKey("SalaId"),
                    j => j
                        .HasOne<ActividadMuseo>()
                        .WithMany()
                        .HasForeignKey("ActividadMuseoId"),
                    j =>
                    {
                        j.ToTable("ActividadSalas");
                        j.HasKey("ActividadMuseoId", "SalaId");
                    });


            // =========================
            // RECURSOS
            // =========================

            builder.HasMany(x => x.Recursos)
                .WithOne(x => x.Actividad)
                .HasForeignKey(x => x.ActividadId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}