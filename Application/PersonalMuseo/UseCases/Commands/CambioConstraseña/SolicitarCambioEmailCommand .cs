using Core.Application;

namespace Application.PersonalMuseo.UseCases.Commands.CambioConstraseña
{
    public class SolicitarCambioEmailCommand : IRequestCommand<bool>
    {
        public string UserId { get; set; } = string.Empty;
        public string NuevoEmail { get; set; } = string.Empty;


    }
}