using Application.Reportes.DataTransferObjets;
using Application.VisitaGrupal.Repositories;
using Core.Application;

namespace Application.Reportes.UseCases.Queries.ReporteGrid
{
    internal sealed class ReporteVisitasGrupalesHandler(
        ICommandQueryBus commandQueryBus,
        IRepositorioVisitaGrupalAutoguiada repositorioVisitaGrupalAutoguiada,
        IRepositorioVisitaGuiada repositorioVisitaGuiada)
        : IRequestQueryHandler<
            ReporteVisitasGrupalesQuery,
            QueryResult<ReporteGridDto>>
    {
        private readonly ICommandQueryBus _commandQueryBus =
            commandQueryBus
            ?? throw new ArgumentNullException(nameof(commandQueryBus));

        private readonly IRepositorioVisitaGrupalAutoguiada _repositorioVisitaGrupalAutoguiada =
            repositorioVisitaGrupalAutoguiada
            ?? throw new ArgumentNullException(nameof(repositorioVisitaGrupalAutoguiada));

        private readonly IRepositorioVisitaGuiada _repositorioVisitaGuiada =
            repositorioVisitaGuiada
            ?? throw new ArgumentNullException(nameof(repositorioVisitaGuiada));

        public async Task<QueryResult<ReporteGridDto>> Handle(
            ReporteVisitasGrupalesQuery request,
            CancellationToken cancellationToken)
        {
            var visitasGrupalesAutoguiadas =
                await _repositorioVisitaGrupalAutoguiada.GetAllGroupVisitAuAsync(
                    request.Desde,
                    request.Hasta);

            var visitasGrupalesGuiadas =
                await _repositorioVisitaGuiada.GetAllGroupVisitAsync(
                    request.Desde,
                    request.Hasta);

            var reporteVisitasGrupalesDto = new List<ReporteGridDto>();

            // Visitas grupales autoguiadas
            foreach (var visita in visitasGrupalesAutoguiadas)
            {
                var slot = visita.Horario;
                    

                reporteVisitasGrupalesDto.Add(new ReporteGridDto
                {
                    Id = visita.Id,
                    Estado = visita.Estado,
                    EstadoConfirmacion = visita.EstadoConfirmacion,
                    Tipo = TipoVisitaGrupal.Autoguiada,

                    CantidadPersonas = visita.CantidadPersonas ?? 0,
                    Institucion = visita.Institucion,

                    Pais = visita.PaisInstitucion,
                    Provincia = visita.ProvinciaInstitucion,
                    Localidad = visita.LocalidadInstitucion,

                    HoraInicio = slot?.Inicio,
                    HoraFin = slot?.Fin
                });
            }

            // Visitas grupales guiadas
            foreach (var visita in visitasGrupalesGuiadas)
            {
                if (visita == null)
                    continue;

                var slot = visita.Horario;

                reporteVisitasGrupalesDto.Add(new ReporteGridDto
                {
                    Id = visita.Id,
                    Estado = visita.Estado,
                    EstadoConfirmacion = visita.EstadoConfirmacion,
                    Tipo = TipoVisitaGrupal.Guiada,

                    CantidadPersonas = visita.CantidadPersonas ?? 0,
                    Institucion = visita.Institucion,

                    Pais = visita.PaisInstitucion,
                    Provincia = visita.ProvinciaInstitucion,
                    Localidad = visita.LocalidadInstitucion,

                    HoraInicio = slot?.Inicio,
                    HoraFin = slot?.Fin
                });
            }

            long totalElementos = reporteVisitasGrupalesDto.Count;
            uint tamañoPagina = (uint)reporteVisitasGrupalesDto.Count;
            uint indicePagina = 1;

            return new QueryResult<ReporteGridDto>(
                reporteVisitasGrupalesDto,
                totalElementos,
                tamañoPagina,
                indicePagina
            );
        }
    }
}