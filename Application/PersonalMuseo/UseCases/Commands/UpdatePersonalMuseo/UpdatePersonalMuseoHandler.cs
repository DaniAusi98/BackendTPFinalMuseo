using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;
using Core.Application;

namespace Application.PersonalMuseo.UseCases.Commands.UpdatePersonalMuseo
{
    internal sealed class UpdatePersonalMuseoHandler(IIdentityService identityService) : IRequestCommandHandler<UpdatePersonalMuseoCommand>
    {
        private readonly IIdentityService _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        public async Task Handle(UpdatePersonalMuseoCommand request, CancellationToken cancellationToken)
        {

            await _identityService.UpdatePersonalMuseoAsync(request.UserId, request.Nombre, request.Apellido, request.Telefono, request.RolId);

        }
    }
}
