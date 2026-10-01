using Application.ActividadesEducativas.DataTransferObjets;
using Core.Application;

namespace Application.ActividadesEducativas.UseCases.Queries.GetActividadesEducativas
{
    public class ActividadesEducativasQuery(DateTime desde, DateTime hasta)
        : QueryRequest<QueryResult<ActividadEducativaDto>>
    {
        public DateTime Desde { get; } = desde;
        public DateTime Hasta { get; } = hasta;
    }
}




