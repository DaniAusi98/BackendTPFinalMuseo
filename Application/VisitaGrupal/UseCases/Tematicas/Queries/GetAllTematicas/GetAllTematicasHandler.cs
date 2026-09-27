using System;
using System.Threading.Tasks;

using Application.MuseumResources.Repositories;
using Application.VisitaGrupal.DataTransferObjets;
using Application.VisitaGrupal.Repositories;

using Core.Application;
using Core.Application.Mapping;

namespace Application.VisitaGrupal.UseCases.Tematicas.Queries.GetAllTematicas
{
    internal class GetAllTematicasHandler(IRepositorioTematicas repositorioTematica) : IRequestQueryHandler<GetAllTematicasQuery, QueryResult<TematicaVisitaDto>>
    {
        private readonly IRepositorioTematicas _repositorioTematica = repositorioTematica ?? throw new ArgumentNullException(nameof(repositorioTematica));
        public async Task<QueryResult<TematicaVisitaDto>> Handle(GetAllTematicasQuery request, CancellationToken cancellationToken)
        {
            IList<Domain.VisitasGrupales.Entities.TematicaVisita> entities = await _repositorioTematica.FindAllAsync();
            return new QueryResult<TematicaVisitaDto>(entities.To<TematicaVisitaDto>(), entities.Count, request.PageIndex, request.PageSize);
        }
    }
}
