using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.VisitaGrupal.DataTransferObjets;
using Application.VisitaGrupal.Repositories;

using Core.Application;
using Core.Application.Mapping;
using Domain.VisitasGrupales.Entities;

namespace Application.VisitaGrupal.UseCases.Queries.GetTematicaVisitaGrupal
{
    internal class GetAllTematicasHandler(IRepositorioTematicas repositorioTematicas) :IRequestQueryHandler<GetAllTematicasQuery, QueryResult<TematicaVisitaDto>>
    {
        private readonly IRepositorioTematicas _repositorioTematicas = repositorioTematicas ?? throw new ArgumentNullException(nameof(repositorioTematicas));
        public async Task<QueryResult<TematicaVisitaDto>> Handle(GetAllTematicasQuery request, CancellationToken cancellationToken)
        {
            IList<TematicaVisita> entities = await _repositorioTematicas.ObtenerDisponiblesAsync();
            return new QueryResult<TematicaVisitaDto>(entities.To<TematicaVisitaDto>(), entities.Count, request.PageIndex, request.PageSize);
        }
    }
}
