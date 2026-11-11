using Application.Eventos.DataTransferObjets;
using Core.Application;

namespace Application.Eventos.UseCases.Queries.ReporteMuestras
{
    public class MuestrasTemporalesQuery(DateTime desde, DateTime? hasta) : QueryRequest<QueryResult<MuestraDto>>
    {
        public DateTime Desde { get; } = desde;
        public DateTime? Hasta { get; } = hasta;
    }
}
