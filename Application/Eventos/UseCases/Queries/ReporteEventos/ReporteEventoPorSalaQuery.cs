using Application.Eventos.DataTransferObjets;
using Core.Application;

namespace Application.Eventos.UseCases.Queries.ReporteEventos
{
    public class ReporteEventosPorSalaQuery(DateTime desde, DateTime hasta) : QueryRequest<QueryResult<EventoPorSalaDto>>
    {
        public DateTime Desde { get; } = desde;
        public DateTime Hasta { get; } = hasta;
    }
    



    
}