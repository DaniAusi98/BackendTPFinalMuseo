
using Application.ApplicationMuseo.ApplicationServices;
using Application.VisitaGrupal.DataTransferObjets;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Core.Application.Mapping;
using Domain.VisitasGrupales.Entities;

namespace Application.VisitaGrupal.UseCases.Queries.GetReservationsByUserId
{

    internal sealed class GetSelfGuidedByUserHandler(IRepositorioVisitaGrupalAutoguiada repositorioVisitaAutoguiada, IClock clock) : IRequestQueryHandler<GetSelfGuidedByUserIdQuery, QueryResult<VisitaAutoguiadaDto>>
    {
        private readonly IRepositorioVisitaGrupalAutoguiada _repositorioVisitaAutoguiada = repositorioVisitaAutoguiada ?? throw new ArgumentNullException(nameof(repositorioVisitaAutoguiada));
        private readonly IClock _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        public async Task<QueryResult<VisitaAutoguiadaDto>> Handle(GetSelfGuidedByUserIdQuery request, CancellationToken cancellationToken)
        {
            var fechaActual = _clock.Now();
            IList<VisitaGrupalAutoguiada> entities = await _repositorioVisitaAutoguiada.ObtenerPorUsuarioIdAsync(request.UsuarioVisitanteId, fechaActual);
            return new QueryResult<VisitaAutoguiadaDto>(entities.To<VisitaAutoguiadaDto>(), entities.Count, request.PageIndex, request.PageSize);
        }

    }


}







