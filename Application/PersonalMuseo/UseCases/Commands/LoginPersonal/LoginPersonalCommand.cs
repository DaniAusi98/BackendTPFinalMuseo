using Application.PersonalMuseo.DataTrasnferObjets;
using Core.Application;
using System.ComponentModel.DataAnnotations;

namespace Application.PersonalMuseo.UseCases.Commands.LoginPersonal
{
    public class LoginPersonalCommand : IRequestCommand<LoginPersonalResponseDto>
    {

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }


        public LoginPersonalCommand()
        {

        }

    }
}
