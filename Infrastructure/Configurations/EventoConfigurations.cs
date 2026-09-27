using Domain.ActividadMuseo.Entities;
using Domain.Eventos.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class EventoConfiguration : IEntityTypeConfiguration<Evento>
    {
        public void Configure(EntityTypeBuilder<Evento> builder)
        {
            builder.ToTable("Eventos");

            // TPT
            builder.HasBaseType<ActividadMuseo>();

            builder.Property(x => x.NombreyApellidoSolicitante)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Institucion)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.TipoEvento)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.TituloEvento)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.DescripcionEvento)
                .HasMaxLength(2000)
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

            // =========================
            // TIPO PUBLICO (lista de enum)
            // =========================

            builder.Property(x => x.TipoPublico)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(
                        v,
                        (System.Text.Json.JsonSerializerOptions?)null),

                    v => string.IsNullOrEmpty(v)
                        ? new List<Domain.Eventos.Enums.Enums.TipoPublico>()
                        : System.Text.Json.JsonSerializer.Deserialize<List<Domain.Eventos.Enums.Enums.TipoPublico>>(
                            v,
                            (System.Text.Json.JsonSerializerOptions?)null)!)
                .HasColumnType("json")
                .IsRequired();


            // =========================
            // URL IMAGENES
            // =========================

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

            builder.OwnsOne(x => x.TelefonoSolicitante, tel =>
            {
                tel.Property(t => t.Valor)
                   .HasColumnName("TelefonoSolicitante")
                   .HasMaxLength(20)
                   .IsRequired();
            });


            // =========================
            // EMAIL SOLICITANTE
            // =========================

            builder.OwnsOne(x => x.EmailSolicitante, email =>
            {
                email.Property(e => e.Valor)
                     .HasColumnName("EmailSolicitante")
                     .HasMaxLength(150)
                     .IsRequired();
            });
        }
    }
}