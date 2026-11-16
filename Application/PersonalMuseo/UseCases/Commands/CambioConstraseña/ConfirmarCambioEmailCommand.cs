using Core.Application;

namespace Application.PersonalMuseo.UseCases.Commands.CambioConstraseña
{
    public class ConfirmarCambioEmailCommand : IRequestCommand<bool>
    {
        public string UserId { get; set; } = string.Empty;
        public string NuevoEmail { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}