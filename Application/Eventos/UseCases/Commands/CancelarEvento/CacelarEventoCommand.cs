using Core.Application;

namespace Application.Eventos.UseCases.Commands.CancelarEvento
{
    public class CancelarEventoCommand : IRequestCommand
    {
        public string EventoId { get; set; }

        public CancelarEventoCommand()
        {

        }

    }
}
