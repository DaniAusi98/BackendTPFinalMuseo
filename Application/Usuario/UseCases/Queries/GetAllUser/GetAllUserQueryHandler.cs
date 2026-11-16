using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;
using Application.Usuario.DataTransferObjets;
using Core.Application;

namespace Application.Usuario.UseCases.Queries.GetAllUser
{
    internal class GetAllUserQueryHandler(IIdentityService identityService) : IRequestQueryHandler<GetAllUserQuery, QueryResult<UserDto>>
    {
        private readonly IIdentityService _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        public async Task<QueryResult<UserDto>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var users = await _identityService.GetPersonalMuseoAsync();
            return new QueryResult<UserDto>(
                users,
                users.Count,
                (uint)users.Count,
                0
            );
        }
    }
}
