using Domain.PersonalMuseo.Entities.UsuarioInterno;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class AsignacionPersonalConfig
        : IEntityTypeConfiguration<AsignacionPersonal>
    {
        public void Configure(
            EntityTypeBuilder<AsignacionPersonal> builder)
        {
            builder.ToTable("AsignacionesPersonal");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.PersonalId)
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(x => x.AreaPuestoId)
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(x => x.FechaAsignacion)
                .IsRequired();

            builder.Property(x => x.Activa)
                .IsRequired();

            builder.HasOne(x => x.Personal)
                .WithMany(x => x.Asignaciones)
                .HasForeignKey(x => x.PersonalId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.AreaPuesto)
                .WithMany()
                .HasForeignKey(x => x.AreaPuestoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new
            {
                x.PersonalId,
                x.AreaPuestoId
            })
            .IsUnique();
        }
    }
}