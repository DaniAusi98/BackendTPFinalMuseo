
using Domain.ActividadMuseo.Entities;
using Domain.Eventos.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.EventoConfiguraciones
{
    public class MuestraTemporalConfiguration
        : IEntityTypeConfiguration<MuestraTemporal>
    {
        public void Configure(EntityTypeBuilder<MuestraTemporal> builder)
        {
            builder.ToTable("MuestrasTemporales");

            // Herencia TPT
            builder.HasBaseType<ActividadBase>();

            builder.Property(x => x.NombreApellidoResponsable)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.TituloPropuesta)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Descripcion)
                .HasMaxLength(2000)
                .IsRequired();

            builder.Property(x => x.FundamentacionObjetivoMuestra)
                .HasMaxLength(2000)
                .IsRequired();

            builder.Property(x => x.Publico)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.Institucion)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(x => x.RequiereDifusion)
                .IsRequired();

            builder.Property(x => x.SolicitaFlyer)
                .IsRequired();

            // Email de la institución: opcional
            builder.OwnsOne(x => x.EmailInstitucion, email =>
            {
                email.Property(e => e.Valor)
                    .HasColumnName("EmailInstitucion")
                    .HasMaxLength(150)
                    .IsRequired();
            });

            builder.Navigation(x => x.EmailInstitucion)
                .IsRequired(false);

            // Teléfono del responsable
            builder.OwnsOne(x => x.TelefonoResponsable, tel =>
            {
                tel.Property(t => t.Valor)
                    .HasColumnName("TelefonoResponsable")
                    .HasMaxLength(20)
                    .IsRequired();
            });

            // Email del responsable
            builder.OwnsOne(x => x.EmailResponsable, email =>
            {
                email.Property(e => e.Valor)
                    .HasColumnName("EmailResponsable")
                    .HasMaxLength(150)
                    .IsRequired();
            });

            // URLs de imágenes
            builder.Property(x => x.UrlImagenesMuestra)
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
                .IsRequired();

            // Logística de la muestra
            builder.OwnsOne(x => x.LogisticaMuestraTemporal, logistica =>
            {
                // Configurar acá las propiedades de LogisticaMuestraTemporal.
            });
        }
    }
}
