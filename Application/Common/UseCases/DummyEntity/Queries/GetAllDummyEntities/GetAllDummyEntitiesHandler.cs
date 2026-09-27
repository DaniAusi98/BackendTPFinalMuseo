using Application.ApplicationMuseo.DataTransferObjects;
using Application.Common.Repositories;
using Application.Common.UseCases.DummyEntity.Queries.GetAllDummyEntities;
using Core.Application;
using Core.Application.Mapping;

namespace Application.ApplicationMuseo.UseCases.DummyEntity.Queries.GetAllDummyEntities
{
    internal class GetAllDummyEntitiesHandler(IDummyEntityRepository context) : IRequestQueryHandler<GetAllDummyEntitiesQuery, QueryResult<DummyEntityDto>>
    {
        private readonly IDummyEntityRepository _context = context ?? throw new ArgumentNullException(nameof(context));


        public async Task<QueryResult<DummyEntityDto>> Handle(GetAllDummyEntitiesQuery request, CancellationToken cancellationToken)
        {
            
            IList<Domain.Common.Entities.DummyEntity> entities = await _context.FindAllAsync();

            return new QueryResult<DummyEntityDto>(entities.To<DummyEntityDto>(), entities.Count, request.PageIndex, request.PageSize);
        }
    }
}
