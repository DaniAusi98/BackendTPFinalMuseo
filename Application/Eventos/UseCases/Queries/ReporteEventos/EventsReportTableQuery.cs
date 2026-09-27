using Application.Eventos.DataTransferObjets;
using Core.Application;

namespace Application.Eventos.UseCases.Queries.ReporteEventos
{
    public class EventsReportTableQuery(DateTime desde, DateTime hasta) : QueryRequest<QueryResult<TablaReporteEventoDto>>
    {
        public DateTime Desde { get; } = desde;
        public DateTime Hasta { get; } = hasta;
    }
    



    
}