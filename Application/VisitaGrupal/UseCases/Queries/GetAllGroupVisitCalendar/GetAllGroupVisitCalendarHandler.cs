using Application.VisitaGrupal.DataTransferObjets;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Core.Application.Mapping;
using Domain.VisitasGrupales.Entities.GrupalAutoguiada;
using Domain.VisitasGrupales.Entities.GrupalGuiada;

namespace Application.VisitaGrupal.UseCases.Queries.GetAllGroupVisitCalendar
{
    internal sealed class GetAllGroupVisitCalendarHandler(
        IRepositorioVisitaGrupalAutoguiada repositorioVisitaGrupalAutoguiada,
        IRepositorioVisitaGuiada repositorioVisitaGuiada
    ) : IRequestQueryHandler<GetAllGroupVisitCalendarQuery, VisitasGrupalesDto>
    {
        private readonly IRepositorioVisitaGuiada _repositorioVisitaGuiada = repositorioVisitaGuiada ?? throw new ArgumentNullException(nameof(repositorioVisitaGuiada));

        private readonly IRepositorioVisitaGrupalAutoguiada _repositorioVisitaGrupalAutoguiada = repositorioVisitaGrupalAutoguiada ?? throw new ArgumentNullException(nameof(repositorioVisitaGrupalAutoguiada));

        public async Task<VisitasGrupalesDto> Handle(GetAllGroupVisitCalendarQuery request, CancellationToken cancellationToken)
        {
            List<VisitaGrupalGuiada> visitasGuiadas = await _repositorioVisitaGuiada.GetAllGroupVisitCalendarAsync(request.FechaConsultaDesde, request.FechaConsultaHasta);
            List<VisitaGrupalAutoguiada> visitasAutoguiadas = await _repositorioVisitaGrupalAutoguiada.GetAllGroupVisitCalendarAsync(request.FechaConsultaDesde, request.FechaConsultaHasta);

            List<VisitaGuiadaDto> visitaGuiadaDtos = [.. visitasGuiadas.To<VisitaGuiadaDto>()];
            List<VisitaAutoguiadaDto> visitaAutoguiadaDtos = [.. visitasAutoguiadas.To<VisitaAutoguiadaDto>()];

            return new VisitasGrupalesDto
            {
                VisitasGuiadas = visitaGuiadaDtos,
                VisitasAutoguiadas = visitaAutoguiadaDtos
            };

        }
    }
}