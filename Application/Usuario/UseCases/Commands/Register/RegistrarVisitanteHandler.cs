using Application.ApplicationMuseo.Constants;
using Application.Exceptions;
using Application.Usuario.ApplicationServices;
using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;
using Application.Usuario.DomainEvents;
using Application.Usuario.UseCases.Commands.Register;

using Core.Application;



namespace Application.Usuario.UseCases.Commands.Register
{
    internal sealed class RegistrarVisitanteHandler(
        IIdentityService identityService,
        IConfirmUserUrl urlGenerator,
        IEmailService emailService
    ) : IRequestCommandHandler<RegistrarVisitanteCommand, string>
    {
        private readonly IIdentityService _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        private readonly IConfirmUserUrl _urlGenerator = urlGenerator ?? throw new ArgumentNullException(nameof(urlGenerator));
        private readonly IEmailService _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        public async Task<string> Handle(RegistrarVisitanteCommand request, CancellationToken cancellationToken)
        {
            if (await _identityService.UserExistsAsync(request.Email)) throw new EntityDoesExistException();

            var userId= await _identityService.RegisterAsync(request.Nombre, request.Apellido, request.FechaNac, request.Email, request.Telefono, request.Password);
            var token = await _identityService.GenerateEmailConfirmationTokenAsync(userId);

            var confirmationUrl = _urlGenerator
        .GetEmailConfirmationUrl(userId, token);


            await _emailService.SendAsync(
                request.Email,
                "Confirmación de correo electrónico",
                $"""
        <h1>Bienvenido {request.Nombre}</h1>

        <p>Para activar tu cuenta hacé clic en el siguiente enlace:</p>

        <a href="{confirmationUrl}">
            Confirmar correo electrónico
        </a>
        """,
                cancellationToken);
            return userId;

        }

       
    }
}

