using Application.ActividadesEducativas.DataTransferObjets;
using Core.Application;

namespace Application.ActividadesEducativas.UseCases.Queries.GetById
{
    public class GetActividadEducativaQuery : IRequestQuery<ActividadEducativaDto>
    {
        public string ActividadId { get; set; }
        public GetActividadEducativaQuery()
        {

        }
    }
}
