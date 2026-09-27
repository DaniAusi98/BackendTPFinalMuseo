using Application.Usuario.DataTransferObjets;
using Core.Application;
using System.ComponentModel.DataAnnotations;

namespace Application.Usuario.UseCases.Commands.LoginUsuario
{
    public class LoginUsuarioCommand : IRequestCommand<LoginResponseDto>
    {

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }


        public LoginUsuarioCommand()
        {

        }

    }
}
