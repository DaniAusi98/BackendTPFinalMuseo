using Core.Application;

using MediatR;

namespace Application.VisitaGrupal.UseCases.Guias.Commands.DeleteGuia
{
    public class DeleteGuiaCommand : IRequestCommand<Unit>
    {
        public string GuiaId { get; set; }
    }
}
