using Core.Application;

using MediatR;

namespace Application.MuseumResources.UseCases.Recurso.Commands.DeleteRecurso
{
    public class DeleteRecursoCommand : IRequestCommand<Unit>
    {
        public int RecursoId { get; set; }
        public DeleteRecursoCommand()
        {
                
        }
    }
}
