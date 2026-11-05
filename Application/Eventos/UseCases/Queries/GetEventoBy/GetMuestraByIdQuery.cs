using Application.Eventos.DataTransferObjets;
using Core.Application;

namespace Application.Eventos.UseCases.Queries.GetEventoBy
{
    public class GetMuestraByIdQuery : IRequestQuery<MuestraDto>
    {
        public string Id { get; set; }
        public GetMuestraByIdQuery()
        {

        }

    }
}
