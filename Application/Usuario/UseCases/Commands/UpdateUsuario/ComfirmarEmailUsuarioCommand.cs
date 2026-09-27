using Core.Application;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Usuario.UseCases.Commands.UpdateUsuario
{
    public class ConfirmarEmailUsuarioCommand : IRequestCommand<string>
    {
        [Required]
        public string Token { get; set; } = string.Empty;
    }
}
