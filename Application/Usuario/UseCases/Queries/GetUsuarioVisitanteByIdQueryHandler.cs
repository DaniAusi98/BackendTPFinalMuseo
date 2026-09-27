using Application.Exceptions;
using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;
using Application.Usuario.DataTransferObjets;

using Core.Application;


namespace Application.Usuario.UseCases.Queries
{
    internal sealed class GetUsuarioVisitanteByIdQueryHandler(IIdentityService identityService ) : IRequestQueryHandler<GetUsuarioVisitanteByIdQueryCommand, UserDto>
    {
        private readonly IIdentityService _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));

        public async Task<UserDto> Handle(GetUsuarioVisitanteByIdQueryCommand request, CancellationToken cancellationToken)
        {
            return await _identityService.FindById(
            request.UsuarioVisitanteId);
        }

    }
}

