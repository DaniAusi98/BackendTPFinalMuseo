using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;
using Application.VisitaGrupal.ApplicationServices;
using Application.VisitaGrupal.DomainEvents;
using MediatR;
using System.Text;


namespace Application.VisitaGrupal.UseCases.Notification
{
    internal sealed class EmailConfirmarVisitaGuiadaHandler(
        IURLConfirmacionVisitaGrupal uRLConfirmacionVisitaGrupal,
        IEmailService emailService,
        IIdentityService identityService) : INotificationHandler<VisitaGuiadaCreated>
    {
        private readonly IURLConfirmacionVisitaGrupal _uRLConfirmacionVisitaGrupal = uRLConfirmacionVisitaGrupal ?? throw new ArgumentNullException(nameof(uRLConfirmacionVisitaGrupal));
        private readonly IEmailService _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        private readonly IIdentityService _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));

        public async Task Handle(VisitaGuiadaCreated notification, CancellationToken cancellationToken)
        {
            // 1. Obtener el email del usuario utilizando su ID de visitante
            var emailUsuario = await _identityService.FindEmailById(notification.UsuarioVisitanteId);

            // Si por alguna razón de consistencia el correo no existe, evitamos que rompa el flujo
            if (string.IsNullOrEmpty(emailUsuario)) return;

            // 2. Generar la URL de confirmación para el frontend
            var confirmationUrl = _uRLConfirmacionVisitaGrupal.GetUrlConfirmacionVisitaGrupal(notification.VisitaId);

            // 3. Evaluar los campos opcionales del ámbito educativo
            var datosEducativosHtml = new StringBuilder();

            if (notification.NivelEducativo.HasValue)
            {
                datosEducativosHtml.Append($"<p><strong>Nivel Educativo:</strong> {notification.NivelEducativo.Value}</p>");
            }

            if (!string.IsNullOrWhiteSpace(notification.AnioGrado))
            {
                datosEducativosHtml.Append(
                    $"<p><strong>Sala / Año / Grado:</strong> {notification.AnioGrado}</p>"
                );
            }

            // 4. Formatear las marcas de tiempo para una lectura natural
            string fechaFormateada = notification.Inicio.ToString("dd/MM/yyyy");
            string horaInicio = notification.Inicio.ToString("HH:mm");
            string horaFin = notification.Fin.ToString("HH:mm");

            // 5. Diseñar la plantilla HTML adaptada con el contenedor de datos y el enlace
            var cuerpoEmailHtml = $@"
            <!DOCTYPE html>
            <html lang='es'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <style>
                    body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0; }}
                    .container {{ max-width: 600px; margin: 20px auto; background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 10px rgba(0,0,0,0.05); }}
                    .header {{ background-color: #1976d2; color: #ffffff; text-align: center; padding: 25px 20px; }}
                    .header h1 {{ margin: 0; font-size: 22px; }}
                    .content {{ padding: 30px 40px; color: #333333; line-height: 1.6; }}
                    .resumen-caja {{ background-color: #f8f9fa; border-left: 4px solid #1976d2; padding: 15px 20px; margin: 20px 0; border-radius: 0 4px 4px 0; }}
                    .resumen-caja p {{ margin: 6px 0; font-size: 14px; }}
                    .button-container {{ text-align: center; margin: 30px 0; }}
                    .btn {{ background-color: #1976d2; color: #ffffff !important; text-decoration: none; padding: 12px 30px; font-size: 16px; font-weight: bold; border-radius: 4px; display: inline-block; }}
                    .footer {{ background-color: #f8f9fa; text-align: center; padding: 15px; font-size: 12px; color: #777777; border-top: 1px solid #eeeeee; }}
                    .fallback-link {{ font-size: 12px; color: #888888; word-break: break-all; margin-top: 20px; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>Confirmación de Visita Grupal</h1>
                    </div>
                    <div class='content'>
                        <p>Se ha registrado una nueva solicitud de visita guiada. A continuación, te presentamos el resumen de la información cargada:</p>
                        
                        <div class='resumen-caja'>
                            <p><strong>Institución:</strong> {notification.NombreInstitucion}</p>
                            {datosEducativosHtml}
                            <p><strong>Cantidad de Personas:</strong> {notification.CantidadPersonas}</p>
                            <p><strong>Fecha de la Visita:</strong> {fechaFormateada}</p>
                            <p><strong>Horario:</strong> de {horaInicio} hs a {horaFin} hs</p>
                        </div>

                        <p>Para validar los datos del grupo y confirmar la reserva de manera definitiva, por favor hacé clic en el siguiente botón:</p>
                        
                        <div class='button-container'>
                            <a href='{confirmationUrl}' class='btn' target='_blank'>Confirmar Asistencia</a>
                        </div>
                        
                        <div class='fallback-link'>
                            <p>Si el botón no funciona, podés copiar y pegar este enlace en tu navegador:</p>
                            <a href='{confirmationUrl}'>{confirmationUrl}</a>
                        </div>
                    </div>
                    <div class='footer'>
                        <p>Este es un correo automático generado por el Sistema de Visitas Guiadas.</p>
                        <p>&copy; {DateTime.UtcNow.Year} Museo - Todos los derechos reservados.</p>
                    </div>
                </div>
            </body>
            </html>";

            // 6. Despachar el correo electrónico a la dirección recuperada
            await _emailService.SendAsync(
                emailUsuario,
                "Confirmación de Reserva de Visita Guiada",
                cuerpoEmailHtml,
                cancellationToken
            );
        }
    }
}
