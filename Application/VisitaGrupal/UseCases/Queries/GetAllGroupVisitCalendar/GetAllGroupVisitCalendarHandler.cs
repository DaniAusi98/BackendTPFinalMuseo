using Application.VisitaGrupal.DataTransferObjets;
using Application.VisitaGrupal.Repositories;
using Core.Application;

namespace Application.VisitaGrupal.UseCases.Queries.GetAllGroupVisitCalendar
{
    internal sealed class GetAllGroupVisitCalendarHandler(
        IRepositorioVisitaGrupalAutoguiada repositorioVisitaGrupalAutoguiada,
        IRepositorioVisitaGuiada repositorioVisitaGuiada
    ) : IRequestQueryHandler<
            GetAllGroupVisitCalendarQuery,
            QueryResult<VisitaGrupalCalendarDto>>
    {
        private readonly IRepositorioVisitaGuiada _repositorioVisitaGuiada =
            repositorioVisitaGuiada
            ?? throw new ArgumentNullException(nameof(repositorioVisitaGuiada));

        private readonly IRepositorioVisitaGrupalAutoguiada _repositorioVisitaGrupalAutoguiada =
            repositorioVisitaGrupalAutoguiada
            ?? throw new ArgumentNullException(nameof(repositorioVisitaGrupalAutoguiada));

        public async Task<QueryResult<VisitaGrupalCalendarDto>> Handle(
            GetAllGroupVisitCalendarQuery request,
            CancellationToken cancellationToken)
        {
            var visitasGuiadas =
                await _repositorioVisitaGuiada.GetAllGroupVisitAsync(
                    request.FechaConsultaDesde,
                    request.FechaConsultaHasta);

            var visitasAutoguiadas =
                await _repositorioVisitaGrupalAutoguiada.GetAllGroupVisitAuAsync(
                    request.FechaConsultaDesde,
                    request.FechaConsultaHasta);

            var visitasGrupalesDto = new List<VisitaGrupalCalendarDto>();

            // Visitas guiadas
            foreach (var visitaGuiada in visitasGuiadas)
            {
                var slot = visitaGuiada.Horario;
                  

                visitasGrupalesDto.Add(new VisitaGrupalCalendarDto
                {
                    Id = visitaGuiada.Id,
                    Tipo = VisitaGrupalCalendarDto.TipoVisitaGrupal.Guiada,

                    Pais = visitaGuiada.PaisInstitucion,
                    Provincia = visitaGuiada.ProvinciaInstitucion,
                    Localidad = visitaGuiada.LocalidadInstitucion,

                    CantidadPersonas = visitaGuiada.CantidadPersonas ?? 0,
                    Institucion = visitaGuiada.Institucion,
                    DiversidadFuncional = visitaGuiada.DiversidadFuncionalDescripcion,

                    HoraInicio = slot?.Inicio,
                    HoraFin = slot?.Fin
                });
            }

            // Visitas autoguiadas
            foreach (var visitaAutoguiada in visitasAutoguiadas)
            {
                var slot = visitaAutoguiada.Horario;
                  

                visitasGrupalesDto.Add(new VisitaGrupalCalendarDto
                {
                    Id = visitaAutoguiada.Id,
                    Tipo = VisitaGrupalCalendarDto.TipoVisitaGrupal.Autoguiada,

                    Pais = visitaAutoguiada.PaisInstitucion,
                    Provincia = visitaAutoguiada.ProvinciaInstitucion,
                    Localidad = visitaAutoguiada.LocalidadInstitucion,

                    CantidadPersonas = visitaAutoguiada.CantidadPersonas ?? 0,
                    Institucion = visitaAutoguiada.Institucion,
                    DiversidadFuncional = visitaAutoguiada.DiversidadFuncional,

                    HoraInicio = slot?.Inicio,
                    HoraFin = slot?.Fin
                });
            }

            long totalElementos = visitasGrupalesDto.Count;
            uint tamañoPagina = (uint)visitasGrupalesDto.Count;
            uint indicePagina = 1;

            return new QueryResult<VisitaGrupalCalendarDto>(
                visitasGrupalesDto,
                totalElementos,
                tamañoPagina,
                indicePagina
            );
        }
    }
}