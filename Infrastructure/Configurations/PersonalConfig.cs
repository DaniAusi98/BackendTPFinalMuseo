using Domain.PersonalMuseo.Entities.UsuarioInterno;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class PersonalConfig : IEntityTypeConfiguration<Personal>
    {
        public void Configure(EntityTypeBuilder<Personal> builder)
        {
            builder.ToTable("Personal");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(x => x.IdentityUserId)
                .HasMaxLength(450)
                .IsRequired();

            builder.Property(x => x.Nombre)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Apellido)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.DNI)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.FechaNacimiento)
                .IsRequired();

            builder.Property(x => x.Activo)
                .IsRequired();

            builder.HasIndex(x => x.IdentityUserId)
                .IsUnique();

            builder.HasIndex(x => x.DNI)
                .IsUnique();

            builder.OwnsOne(x => x.Email, email =>
            {
                email.Property(x => x.Valor)
                    .HasColumnName("Email")
                    .HasMaxLength(254)
                    .IsRequired();
            });

            builder.OwnsOne(x => x.Telefono, telefono =>
            {
                telefono.Property(x => x.Valor)
                    .HasColumnName("Telefono")
                    .HasMaxLength(10)
                    .IsRequired();
            });

            builder.HasMany(x => x.Asignaciones)
                .WithOne(x => x.Personal)
                .HasForeignKey(x => x.PersonalId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}