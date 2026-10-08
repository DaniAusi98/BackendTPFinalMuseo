using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;
using Application.Usuario.DataTransferObjets;
using Core.Application;

namespace Application.Usuario.UseCases.Queries.GetUsuarioById
{
    internal sealed class GetUserByIdHandler(IIdentityService identityService) : IRequestQueryHandler<GetUserByIdQuery, UserDto>
    {
        private readonly IIdentityService _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));

        public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {

            return await _identityService.FindById(
                  request.UsuarioId);
        }
    }
}
