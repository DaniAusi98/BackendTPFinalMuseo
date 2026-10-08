using Application.Common.ApplicationServices;

namespace Application.PersonalMuseo.ApplicationServices
{
    public class UserConfirmationEmailService(IEmailService emailService) : IUserConfirmationEmailService
    {
        private readonly IEmailService emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        public async Task SendUserConfirmationEmailAsync(string userEmail, string confirmationUrl)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(userEmail);
            ArgumentException.ThrowIfNullOrWhiteSpace(confirmationUrl);
            var html = $"""
          

            <p>Para activar tu cuenta, haz clic en el siguiente enlace:</p>

            <p>
                <a href="{confirmationUrl}"
                   style="background-color:#2563eb;color:white;padding:10px 16px;text-decoration:none;border-radius:5px;">
                    Confirmar correo electrónico
                </a>
            </p>

            <p>Si no solicitaste esta cuenta, puedes ignorar este correo.</p>
            """;

            await emailService.SendAsync(userEmail, "Correo de confirmacion", html);
        }
    }

}
