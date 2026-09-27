using Application.Eventos.DataTransferObjets;
using Core.Application;

namespace Application.Eventos.UseCases.Queries.ReporteEventos
{
    public class ReporteEventosPorTipoQuery(DateTime desde, DateTime hasta) : QueryRequest<QueryResult<EventosPorTipoDto>>
    {
        public DateTime Desde { get; } = desde;
        public DateTime Hasta { get; } = hasta;
    }
    
    
}