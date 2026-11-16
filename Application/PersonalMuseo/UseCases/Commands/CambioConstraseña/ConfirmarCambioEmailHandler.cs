using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;
using Core.Application;

namespace Application.PersonalMuseo.UseCases.Commands.CambioConstraseña
{
    internal sealed class ConfirmarCambioEmailHandler(
        IIdentityService identityService)
        : IRequestCommandHandler<ConfirmarCambioEmailCommand, bool>
    {
        private readonly IIdentityService _identityService =
            identityService ?? throw new ArgumentNullException(nameof(identityService));

        public async Task<bool> Handle(
            ConfirmarCambioEmailCommand request,
            CancellationToken cancellationToken)
        {
            return await _identityService.ChangeEmailAsync(
                request.UserId,
                request.NuevoEmail,
                request.Token
            );
        }
    }
}