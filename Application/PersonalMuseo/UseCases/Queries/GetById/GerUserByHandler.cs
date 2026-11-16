using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;
using Application.Usuario.DataTransferObjets;
using Core.Application;

namespace Application.PersonalMuseo.UseCases.Queries.GetById
{
    internal sealed class GerUserByHandler(IIdentityService identityService) : IRequestQueryHandler<GetUserByQuery, UserDto>
    {
        private readonly IIdentityService _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        public async Task<UserDto> Handle(GetUserByQuery request, CancellationToken cancellationToken)
        {
            return await _identityService.FindById(request.UserId);
        }
    }
}
