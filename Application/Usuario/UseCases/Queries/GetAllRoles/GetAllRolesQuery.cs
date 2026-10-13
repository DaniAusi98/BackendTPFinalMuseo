using Application.Usuario.DataTransferObjets;
using Core.Application;

namespace Application.Usuario.UseCases.Queries.GetAllRoles
{
    public class GetAllRolesQuery : QueryRequest<QueryResult<RolDto>>
    {
    }
}
