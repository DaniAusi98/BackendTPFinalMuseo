using Core.Application;
using MediatR;

namespace Application.Eventos.UseCases.Commands.EliminarPorpuestaEvento
{
    public class DeletePropuestaEventoCommand : IRequestCommand<Unit>
    {
        public string PropuestaId { get; set; }
        public DeletePropuestaEventoCommand()
        {

        }
    }
}
