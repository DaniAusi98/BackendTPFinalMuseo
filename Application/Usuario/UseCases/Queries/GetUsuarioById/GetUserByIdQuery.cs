using Application.Usuario.DataTransferObjets;
using Core.Application;

namespace Application.Usuario.UseCases.Queries.GetUsuarioById
{
    public class GetUserByIdQuery : IRequestQuery<UserDto>
    {
        public string UsuarioId { get; set; }
        public GetUserByIdQuery()
        {

        }

    }
}
