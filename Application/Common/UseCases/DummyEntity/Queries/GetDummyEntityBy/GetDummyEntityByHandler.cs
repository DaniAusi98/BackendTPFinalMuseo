using Application.ApplicationMuseo.DataTransferObjects;
using Application.Common.Repositories;
using Application.Exceptions;

using Core.Application;
using Core.Application.Mapping;

namespace Application.ApplicationMuseo.UseCases.DummyEntity.Queries.GetDummyEntityBy
{
    internal sealed class GetDummyEntityByHandler(IDummyEntityRepository context) : IRequestQueryHandler<GetDummyEntityByQuery, DummyEntityDto>
    {
        private readonly IDummyEntityRepository _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<DummyEntityDto> Handle(GetDummyEntityByQuery request, CancellationToken cancellationToken)
        {
            Domain.Common.Entities.DummyEntity entity = await _context.FindOneAsync(request.DummyIdProperty) ?? throw new EntityDoesNotExistException();
            return entity.To<DummyEntityDto>();
        }
    }
}
