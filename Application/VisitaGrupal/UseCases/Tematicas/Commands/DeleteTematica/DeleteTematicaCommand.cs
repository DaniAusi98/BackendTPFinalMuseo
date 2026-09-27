using Core.Application;

using MediatR;

namespace Application.VisitaGrupal.UseCases.Tematicas.Commands.DeleteTematica
{
    public class DeleteTematicaCommand : IRequestCommand<Unit>
    {
        public int TematicaId { get; set; }
    }
}
