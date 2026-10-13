using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;
using Application.Usuario.DataTransferObjets;
using Core.Application;

namespace Application.Usuario.UseCases.Queries.GetAllRoles
{
    internal sealed class GetAllRolesHandler(IIdentityService identityService) : IRequestQueryHandler<GetAllRolesQuery, QueryResult<RolDto>>
    {
        private readonly IIdentityService _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        public async Task<QueryResult<RolDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            RolDto[] rolesDto = await _identityService.GetAllRolesAsync();
            return new QueryResult<RolDto>(rolesDto, rolesDto.Length, request.PageIndex, request.PageSize);
        }
    }
}
