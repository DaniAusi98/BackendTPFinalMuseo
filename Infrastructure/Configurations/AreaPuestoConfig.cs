using Domain.PersonalMuseo.Entities.UsuarioInterno;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class AreaPuestoConfig : IEntityTypeConfiguration<AreaPuesto>
    {
        public void Configure(EntityTypeBuilder<AreaPuesto> builder)
        {
            builder.ToTable("AreaPuesto");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(x => x.AreaId)
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(x => x.PuestoId)
                .HasMaxLength(36)
                .IsRequired();



            builder.HasOne(x => x.Area)
                .WithMany()
                .HasForeignKey(x => x.AreaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Puesto)
                .WithMany()
                .HasForeignKey(x => x.PuestoId)
                .OnDelete(DeleteBehavior.Restrict);



        }
    }
}