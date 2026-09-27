/*using Domain.Usuarios.Entities.UsuarioVisitante;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class UsuarioVisitanteConfig : IEntityTypeConfiguration<Visitante>
    {
        public void Configure(EntityTypeBuilder<Visitante> builder)
        {
            // Nombre de la tabla
            builder.ToTable("UsuarioVisitante");

            // Clave primaria
            builder.HasKey(u => u.Id);

            // Value Object Nombre
            builder.Property(u => u.Nombre)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(u => u.Apellido)
                .HasMaxLength(200)
                .IsRequired();



            // Value Object Email
            builder.OwnsOne(u => u.EmailVisitante, email =>
            {
                email.Property(e => e.Valor)
                     .HasColumnName("EmailVisitante")
                     .HasMaxLength(150)
                     .IsRequired();
            });

            // Value Object Telefono
            builder.OwnsOne(u => u.TelefonoVisitante, tel =>
            {
                tel.Property(t => t.Valor)
                   .HasColumnName("TelefonoVisitante")
                   .HasMaxLength(20)
                   .IsRequired();
            });

            // Fecha de nacimiento
            builder.Property(u => u.FechaNac)
                   .IsRequired();

            // Relación con VisitaGuiada


            // Value Object PasswordHash
            builder.OwnsOne(u => u.PasswordHash, pw =>
            {
                pw.Property(p => p.Valor)
                  .HasColumnName("PasswordHash")
                  .IsRequired();
            });

            // EmailToken VO (TokenConfirmacion)
            builder.OwnsOne(u => u.TokenConfirmacion, token =>
            {
                token.Property(t => t.Token)
                     .HasColumnName("TokenConfirmacion")
                     .HasMaxLength(200)
                     .IsRequired(); // siempre required si el VO existe

                token.Property(t => t.ExpiracionUtc)
                     .HasColumnName("TokenExpiracion")
                     .IsRequired(); // siempre required si el VO existe
            });
        }
    }
}
*/
