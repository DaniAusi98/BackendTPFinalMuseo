using Core.Application;
using MediatR;

namespace Application.Eventos.UseCases.Commands.DeletePropuestaMuestra
{
    public class DeletePropuestaMuestraCommand : IRequestCommand<Unit>
    {
        public string PropuestaId { get; set; }
        public DeletePropuestaMuestraCommand()
        {

        }
    }
}
