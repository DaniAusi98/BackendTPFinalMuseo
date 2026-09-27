using Application.Exceptions;
using Application.MuseumResources.DataTransferObjects;
using Application.MuseumResources.Repositories;
using Core.Application;
using Core.Application.Mapping;

namespace Application.MuseumResources.UseCases.MuseumGallery.Queries.GetSalaBy
{
    internal sealed class GetSalaByHandler(IRepositorioSala repositorioSala) : IRequestQueryHandler<GetMuseumGalleryByQuery,SalaDto>
    {
        private readonly IRepositorioSala _context = repositorioSala ?? throw new ArgumentNullException(nameof(repositorioSala));
        public async Task<SalaDto> Handle(GetMuseumGalleryByQuery request, CancellationToken cancellationToken)
        {
            Domain.RecursoMuseo.Entities.Sala entity = await _context.FindOneAsync(request.SalaId) ?? throw new EntityDoesNotExistException();
            return entity.To<SalaDto>();
        }
    }
    
    
}
/*internal sealed class GetDummyEntityByHandler(IDummyEntityRepository context) : IRequestQueryHandler<GetDummyEntityByQuery, DummyEntityDto>
{
    private readonly IDummyEntityRepository _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<DummyEntityDto> Handle(GetDummyEntityByQuery request, CancellationToken cancellationToken)
    {
        Domain.Common.Entities.DummyEntity entity = await _context.FindOneAsync(request.DummyIdProperty) ?? throw new EntityDoesNotExistException();
        return entity.To<DummyEntityDto>();
    }
}*/
