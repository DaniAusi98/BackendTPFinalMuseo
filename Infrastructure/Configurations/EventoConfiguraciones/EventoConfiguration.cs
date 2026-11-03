using Domain.ActividadMuseo.Entities;
using Domain.Eventos.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.EventoConfiguraciones
{
    public class EventoConfiguration : IEntityTypeConfiguration<Evento>
    {
        public void Configure(EntityTypeBuilder<Evento> builder)
        {
            builder.ToTable("Eventos");

            // TPT
            builder.HasBaseType<ActividadBase>();

            builder.Property(x => x.NombreyApellidoResponsable)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Institucion)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(x => x.TipoEvento)
                .HasConversion<string>()
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Titulo)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.DescripcionEvento)
                .HasMaxLength(2000)
                .IsRequired(false);
            builder.Property(x => x.TipoPublico)
               .HasMaxLength(200)
               .IsRequired(false);

            builder.Property(x => x.FundamentacionEvento)
                .HasMaxLength(2000)
                .IsRequired(false);

            builder.Property(x => x.CantidadEstimada)
                .IsRequired(false);


            builder.Property(x => x.RequiereDifusion)
                .IsRequired();
            builder.Property(x => x.SolicitaFlyer)
                .IsRequired();


            builder.Property(x => x.UrlImagenes)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(
                        v,
                        (System.Text.Json.JsonSerializerOptions?)null),

                    v => string.IsNullOrEmpty(v)
                        ? new List<string>()
                        : System.Text.Json.JsonSerializer.Deserialize<List<string>>(
                            v,
                            (System.Text.Json.JsonSerializerOptions?)null)!)
                .HasColumnType("json")
                .IsRequired(false);


            // =========================
            // TELEFONO SOLICITANTE
            // =========================

            builder.OwnsOne(x => x.TelefonoResponsable, tel =>
            {
                tel.Property(t => t.Valor)
                   .HasColumnName("TelefonoSolicitante")
                   .HasMaxLength(20)
                   .IsRequired();
            });


            // =========================
            // EMAIL SOLICITANTE
            // =========================

            builder.OwnsOne(x => x.EmailResponsable, email =>
            {
                email.Property(e => e.Valor)
                     .HasColumnName("EmailSolicitante")
                     .HasMaxLength(150)
                     .IsRequired();
            });
        }
    }
}