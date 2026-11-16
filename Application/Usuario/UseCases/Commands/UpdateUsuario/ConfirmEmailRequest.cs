using Core.Application;

namespace Application.Usuario.UseCases.Commands.UpdateUsuario
{
    public class ConfirmEmailRequest : IRequestCommand<bool>
    {
        public string UserId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public ConfirmEmailRequest()
        {

        }
    }
}
