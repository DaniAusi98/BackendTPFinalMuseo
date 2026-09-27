using Application.ApplicationMuseo.DataTransferObjects;
using Core.Application;

namespace Application.Common.UseCases.DummyEntity.Queries.GetAllDummyEntities
{
    public class GetAllDummyEntitiesQuery : QueryRequest<QueryResult<DummyEntityDto>>
    {
    }
}
