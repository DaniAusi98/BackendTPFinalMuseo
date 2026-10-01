using Application.ActividadesEducativas.DataTransferObjets;
using Core.Application;
using System;
namespace Application.ActividadesEducativas.UseCases.Queries.GetActEducacion
{
    public class ActividadesEducacionQuery(DateTime desde, DateTime hasta)
        : IRequestQuery<CalendarEducacionDto>
    {
        public DateTime Desde { get; } = desde;
        public DateTime Hasta { get; } = hasta;
    }
}
