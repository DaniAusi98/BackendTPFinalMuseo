using Application.Usuario.DataTransferObjets;
using Core.Application;

namespace Application.Usuario.UseCases.Queries.GetAllUser
{
    public class GetAllUserQuery : QueryRequest<QueryResult<UserDto>>
    {
    }
}
