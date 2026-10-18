using Domain.VisitasGrupales.Entities.GrupalGuiada.Guia;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.VisitasGrupales
{
    public class GuiaConfiguration : IEntityTypeConfiguration<Guia>
    {
        public void Configure(EntityTypeBuilder<Guia> builder)
        {
            builder.ToTable("Guias");

            builder.HasKey(g => g.Id);

            builder.Property(g => g.Nombre)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(g => g.Apellido)
                .HasMaxLength(100)
                .IsRequired();

            // Value Object Email
            builder.OwnsOne(g => g.Email, email =>
            {
                email.Property(e => e.Valor)
                     .HasColumnName("Email")
                     .HasMaxLength(200)
                     .IsRequired();
            });

            // Value Object Telefono
            builder.OwnsOne(g => g.Telefono, telefono =>
            {
                telefono.Property(t => t.Valor)
                        .HasColumnName("Telefono")
                        .HasMaxLength(30)
                        .IsRequired();
            });

            builder.Property(g => g.Activo)
                .IsRequired();

            builder.Property(g => g.UserId)
                .IsRequired(false);

            builder.HasMany(g => g.HorariosGuia)
                .WithOne(h => h.Guia)
                .HasForeignKey(h => h.GuiaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(g => g.AusenciasProgramadas)
                .WithOne(a => a.Guia)
                .HasForeignKey(a => a.GuiaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}