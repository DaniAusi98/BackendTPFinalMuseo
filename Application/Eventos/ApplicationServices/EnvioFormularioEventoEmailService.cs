using Application.Common.ApplicationServices;
namespace Application.Eventos.ApplicationServices
{
    internal class EnvioFormularioEventoEmailService(IEmailService emailService) : IEnvioFormularioEventoEmailService
    {
        private readonly IEmailService emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));

        public async Task SendEnvioFormularioEmailAsync(string emailResponsable, string tituloPropuesta, string urlFormulario)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(emailResponsable);
            ArgumentException.ThrowIfNullOrWhiteSpace(tituloPropuesta);
            ArgumentException.ThrowIfNullOrWhiteSpace(urlFormulario);
            var html = $"""
          

            <p>La Propuesta con el titulo "{tituloPropuesta}" ha sido aprobada , haz clic en el siguiente enlace para completar el formulario:</p>

            <p>
                <a href="{urlFormulario}"
                   style="background-color:#2563eb;color:white;padding:10px 16px;text-decoration:none;border-radius:5px;">
                </a>
            </p>

            <p>Por cualquier consulta , comunicarse por email con el area de recepcion del Museo.Saludos Cordiales</p>
            """;

            await emailService.SendAsync(emailResponsable, "Propuesta de evento y actividades Externas", html);
        }
    }
}
