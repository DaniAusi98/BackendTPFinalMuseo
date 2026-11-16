using Application.Usuario.DataTransferObjets;
using Core.Application;

namespace Application.PersonalMuseo.UseCases.Queries.GetById
{
    public class GetUserByQuery : IRequestQuery<UserDto>
    {
        public string UserId { get; set; }

        public GetUserByQuery()
        {

        }
    }
}
