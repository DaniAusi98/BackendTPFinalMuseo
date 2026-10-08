using Domain.PersonalMuseo.Entities.UsuarioInterno;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class AreaConfig : IEntityTypeConfiguration<Area>
    {
        public void Configure(EntityTypeBuilder<Area> builder)
        {
            builder.ToTable("Areas");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(x => x.Nombre)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Descripcion)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.Activo)
                .IsRequired();

            builder.HasIndex(x => x.Nombre)
                .IsUnique();
        }
    }
}