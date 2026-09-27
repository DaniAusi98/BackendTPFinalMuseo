using Application.Usuario.DataTransferObjets;

using Core.Application;

namespace Application.Usuario.UseCases.Queries
{
    public class GetUsuarioVisitanteByIdQueryCommand : IRequestQuery<UserDto>
    {
        public string UsuarioVisitanteId { get; set; } = string.Empty;
        public GetUsuarioVisitanteByIdQueryCommand()
        {

        }
    }
}
