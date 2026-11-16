
using Application.Common.ApplicationServices;
using Application.PersonalMuseo.ApplicationServices;
using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;
using Core.Application;
using Domain.Common.Exceptions;

namespace Application.PersonalMuseo.UseCases.Commands.CambioConstraseña
{
    internal sealed class SolicitarCambioEmailHandler(
        IIdentityService identityService,
        IConfirmChangeEmailUrlDashboard confirmChangeEmailUrlDashboard,
        IEmailService emailService)
        : IRequestCommandHandler<SolicitarCambioEmailCommand, bool>
    {
        private readonly IIdentityService _identityService =
            identityService ?? throw new ArgumentNullException(nameof(identityService));

        private readonly IConfirmChangeEmailUrlDashboard _confirmChangeEmailUrlDashboard =
            confirmChangeEmailUrlDashboard ?? throw new ArgumentNullException(nameof(confirmChangeEmailUrlDashboard));

        private readonly IEmailService _emailService =
            emailService ?? throw new ArgumentNullException(nameof(emailService));

        public async Task<bool> Handle(
            SolicitarCambioEmailCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _identityService.FindById(request.UserId);

            if (user is null)
                throw new DomainException("El usuario no existe.");

            if (user.Email.Equals(request.NuevoEmail, StringComparison.OrdinalIgnoreCase))
                throw new DomainException("El nuevo correo electrónico debe ser diferente al actual.");

            var emailExiste = await _identityService.UserExistsAsync(request.NuevoEmail);

            if (emailExiste)
                throw new DomainException("El correo electrónico ya está registrado.");

            var token = await _identityService.GenerateChangeEmailTokenAsync(
                request.UserId,
                request.NuevoEmail
            );

            var url = _confirmChangeEmailUrlDashboard.GetChangeEmailConfirmationUrl(
                request.UserId,
                request.NuevoEmail,
                token
            );

            var html = $"""
                <h2>Cambio de correo electrónico</h2>

                <p>Recibimos una solicitud para cambiar el correo electrónico de tu cuenta.</p>

                <p>Para confirmar el cambio, hacé clic en el siguiente enlace:</p>

                <p>
                    <a href="{url}"
                       style="background-color:#2563eb;color:white;padding:10px 16px;text-decoration:none;border-radius:5px;">
                        Confirmar cambio de correo electrónico
                    </a>
                </p>

                <p>Si no solicitaste este cambio, podés ignorar este correo.</p>
                """;

            await _emailService.SendAsync(
                request.NuevoEmail,
                "Confirmación de cambio de correo electrónico",
                html,
                cancellationToken
            );

            return true;
        }
    }
}
