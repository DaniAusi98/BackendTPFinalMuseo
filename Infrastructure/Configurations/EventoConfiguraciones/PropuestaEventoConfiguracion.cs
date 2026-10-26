using Domain.Eventos.Entities;
using Domain.RecursoMuseo.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.EventoConfiguraciones
{
    public class PropuestaEventoConfiguracion
        : IEntityTypeConfiguration<PropuestaEvento>
    {
        public void Configure(EntityTypeBuilder<PropuestaEvento> builder)
        {
            builder.ToTable("PropuestaEvento");

            builder.Property(x => x.NombreApellidoResponsable)
                .HasMaxLength(200)
                .IsRequired();

            builder.OwnsOne(x => x.TelefonoResponsable, tel =>
            {
                tel.Property(t => t.Valor)
                    .HasColumnName("TelefonoResponsable")
                    .HasMaxLength(20)
                    .IsRequired();
            });

            builder.OwnsOne(x => x.EmailResponsable, email =>
            {
                email.Property(e => e.Valor)
                    .HasColumnName("EmailResponsable")
                    .HasMaxLength(150)
                    .IsRequired();
            });

            builder.Property(x => x.TituloPropuesta)
                .IsRequired();

            builder.Property(x => x.TipoEvento)
                .HasConversion<string>()
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Fecha)
                .IsRequired();

            builder.Property(x => x.FechaHasta)
                .IsRequired();

            builder.Property(x => x.FormularioEventoCompletado)
                .IsRequired();

            builder.HasMany(x => x.Salas)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "PropuestaSala",
                    j => j
                        .HasOne<Sala>()
                        .WithMany()
                        .HasForeignKey("SalaId"),
                    j => j
                        .HasOne<PropuestaEvento>()
                        .WithMany()
                        .HasForeignKey("PropuestaId"),
                    j =>
                    {
                        j.ToTable("PropuestaSalas");
                        j.HasKey("PropuestaId", "SalaId");
                    });

            builder.Property(x => x.RRule)
                .HasColumnName("RRule")
                .HasMaxLength(255)
                .IsRequired(false);

            builder.HasMany(x => x.BloqueosSala)
                .WithOne()
                .HasForeignKey(x => x.PropuestaEventoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.FormularioEnviado)
                .IsRequired();

            builder.Property(x => x.FechaEnvioFormulario)
                .IsRequired(false);
        }
    }
}