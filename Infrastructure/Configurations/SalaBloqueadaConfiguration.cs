using Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class BloqueoSalaConfiguration : IEntityTypeConfiguration<BloqueoSala>
    {
        public void Configure(EntityTypeBuilder<BloqueoSala> builder)
        {
            builder.ToTable("BloqueosSala");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FechaDesde)
                .IsRequired();

            builder.Property(x => x.FechaHasta)
                .IsRequired();

            builder.Property(x => x.Motivo)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Observaciones)
                .HasMaxLength(500)
                .IsRequired(false);


            builder.HasOne(x => x.Sala)
                .WithMany()
                .HasForeignKey(x => x.SalaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
