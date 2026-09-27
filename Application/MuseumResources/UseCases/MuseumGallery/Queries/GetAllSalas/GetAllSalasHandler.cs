using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.MuseumResources.DataTransferObjects;
using Application.MuseumResources.Repositories;

using Core.Application;
using Core.Application.Mapping;

namespace Application.MuseumResources.UseCases.MuseumGallery.Queries.GetAllSalas
{
    internal class GetAllSalasHandler(IRepositorioSala repositorioSala) : IRequestQueryHandler<GetAllMuseumGalleriesQuery, QueryResult<SalaDto>>
    {
        private readonly IRepositorioSala _repositorioSala = repositorioSala ?? throw new ArgumentNullException(nameof(repositorioSala));
        public async Task<QueryResult<SalaDto>> Handle(GetAllMuseumGalleriesQuery request, CancellationToken cancellationToken)
        {
            IList<Domain.RecursoMuseo.Entities.Sala> salas =await _repositorioSala.FindAllAsync();
            return new QueryResult<SalaDto>(salas.To<SalaDto>(), salas.Count, request.PageIndex, request.PageSize);

        }
    }
}
/*using Application.ApplicationMuseo.DataTransferObjects;
using Application.ApplicationMuseo.Repositories;

using Core.Application;

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
*/
