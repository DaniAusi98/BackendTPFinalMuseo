using Application.PersonalMuseo.DataTrasnferObjets;
using Core.Application;

namespace Application.PersonalMuseo.UseCases.Queries.ObtenerPuestosArea
{
    public class ObtenerPuestoAreaQuery(string areaId) : QueryRequest<QueryResult<AreaPuestoDto>>
    {
        public string AreaId { get; private set; } = areaId;
    }
}
