using System;
using System.Threading.Tasks;

using Application.Exceptions;
using Application.MuseumResources.Repositories;
using Application.VisitaGrupal.DataTransferObjets;
using Application.VisitaGrupal.Repositories;

using Core.Application;
using Core.Application.Mapping;

namespace Application.VisitaGrupal.UseCases.Tematicas.Queries.GetTematicaBy
{
    internal sealed class GetTematicaByHandler(IRepositorioTematicas repositorioTematica) : IRequestQueryHandler<GetTematicaByQuery, TematicaVisitaDto>
    {
        private readonly IRepositorioTematicas _context = repositorioTematica ?? throw new ArgumentNullException(nameof(repositorioTematica));
        public async Task<TematicaVisitaDto> Handle(GetTematicaByQuery request, CancellationToken cancellationToken)
        {
            Domain.VisitasGrupales.Entities.TematicaVisita entity = await _context.FindOneAsync(request.TematicaId) ?? throw new EntityDoesNotExistException();
            return entity.To<TematicaVisitaDto>();
        }
    }
}
