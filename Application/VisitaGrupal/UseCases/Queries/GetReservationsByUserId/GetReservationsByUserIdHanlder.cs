using Application.ApplicationMuseo.ApplicationServices;
using Application.VisitaGrupal.DataTransferObjets;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Core.Application.Mapping;
using Domain.VisitasGrupales.Entities.GrupalGuiada;

namespace Application.VisitaGrupal.UseCases.Queries.GetReservationsByUserId
{
    internal class GetReservationsByUserIdHanlder(IRepositorioVisitaGuiada repositorioVisitaGuiada,IClock clock ) : IRequestQueryHandler<GetReservationsByUserIdQuery, QueryResult<GuidedTourReservationDto>>
    {
        private readonly IRepositorioVisitaGuiada _repositorioVisitaGuiada = repositorioVisitaGuiada ?? throw new ArgumentNullException(nameof(repositorioVisitaGuiada));
        private readonly IClock _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        public async Task<QueryResult<GuidedTourReservationDto>> Handle(GetReservationsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var fechaActual = _clock.Now();
            IList<VisitaGrupalGuiada> entities = await _repositorioVisitaGuiada.ObtenerPorUsuarioIdAsync(request.UsuarioVisitanteId,fechaActual);
            return new QueryResult<GuidedTourReservationDto>(entities.To<GuidedTourReservationDto>(), entities.Count, request.PageIndex, request.PageSize);
        }
    }
}
