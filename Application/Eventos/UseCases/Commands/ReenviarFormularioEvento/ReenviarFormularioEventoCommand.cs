using Core.Application;

namespace Application.Eventos.UseCases.Commands.ReenviarFormularioEvento
{
    public class ReenviarFormularioEventoCommand : IRequestCommand
    {
        public string IdEvento { get; set; }
        public ReenviarFormularioEventoCommand()
        {

        }
    }
}
