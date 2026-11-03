using Application.Eventos.DataTransferObjets;
using Core.Application;

namespace Application.Eventos.UseCases.Queries.GetEventoBy
{
    public class GetEventoByIdQuery : IRequestQuery<EventoDto>
    {
        public string EventoId { get; set; }
        public GetEventoByIdQuery()
        {

        }
    }
}
