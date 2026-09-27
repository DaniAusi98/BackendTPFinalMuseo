using Domain.VisitasGrupales.Entities;

using Domain.RecursoMuseo.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.VisitasGrupales
{
    public class TematicaVisitaConfiguration : IEntityTypeConfiguration<TematicaVisita>
    {
        public void Configure(EntityTypeBuilder<TematicaVisita> builder)
        {
            builder.ToTable("TematicasVisita");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Nombre)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Descripcion)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(x => x.Disponible)
                .IsRequired();


            // Relación TematicaVisita - Sala
            builder.HasMany(x => x.Salas)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "TematicaSala",
                    j => j
                        .HasOne<Sala>()
                        .WithMany()
                        .HasForeignKey("SalaId")
                        .OnDelete(DeleteBehavior.Cascade),

                    j => j
                        .HasOne<TematicaVisita>()
                        .WithMany()
                        .HasForeignKey("TematicaVisitaId")
                        .OnDelete(DeleteBehavior.Cascade),

                    j =>
                    {
                        j.ToTable("TematicaSalas");

                        j.HasKey(
                            "TematicaVisitaId",
                            "SalaId");
                    });
        }
    }
}
