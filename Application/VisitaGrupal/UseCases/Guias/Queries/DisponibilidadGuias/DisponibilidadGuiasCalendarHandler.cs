using Application.ConfiguracionSistema.Repositories;
using Application.VisitaGrupal.DataTransferObjets;
using Application.VisitaGrupal.DataTransferObjets.Guias;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Core.Application.Mapping;
using Domain.VisitasGrupales.DomainServices;

namespace Application.VisitaGrupal.UseCases.Guias.Queries.DisponibilidadGuias
{
    internal class DisponibilidadGuiasCalendarHandler(
        IRepositorioGuia repositorioGuia,
        IRepositorioConfiguracionVisitasGrupalesGuiadas repositorioConfiguracion,
        IRepositorioCalendarioMuseo repositorioCalendario,
        IDisponibilidadGuia disponibilidadGuia)
        : IRequestQueryHandler<DisponibilidadGuiasCalendarQuery, DisponibilidadGuiasDto>
    {
        private readonly IRepositorioGuia _repositorioGuia =
            repositorioGuia ?? throw new ArgumentNullException(nameof(repositorioGuia));

        private readonly IRepositorioConfiguracionVisitasGrupalesGuiadas _repositorioConfiguracion =
            repositorioConfiguracion;

        private readonly IRepositorioCalendarioMuseo _repositorioCalendario =
            repositorioCalendario;

        private readonly IDisponibilidadGuia _disponibilidadGuia =
            disponibilidadGuia ?? throw new ArgumentNullException(nameof(disponibilidadGuia));

        public async Task<DisponibilidadGuiasDto> Handle(
            DisponibilidadGuiasCalendarQuery request,
            CancellationToken cancellationToken)
        {
            var guias = await _repositorioGuia.ObtenerDisponibilidadGuias(
                request.FechaDesde,
                request.FechaHasta);

            var configuracion = await _repositorioConfiguracion
                .ObtenerConfiguracionActivaAsync();

            var calendario = await _repositorioCalendario
                .ObtenerCalendarioActivoAsync();

            var guiasDisponibles = new List<DisponibilidadGuiaDto>();

            foreach (var guia in guias)
            {
                var fechasDisponibles =
                    await _disponibilidadGuia.CalcularDisponibilidadGuia(
                        request.FechaDesde,
                        request.FechaHasta,
                        guia,
                        configuracion,
                        calendario);

                guiasDisponibles.Add(new DisponibilidadGuiaDto
                {
                    GuiaId = guia.Id,
                    Nombre = guia.Nombre,
                    Apellido = guia.Apellido,

                    HorariosLaborales = guia.HorariosGuia
                        .Select(h => h.To<GuiaHorarioDto>())
                        .ToList(),

                    FechasDisponiblesDtos = fechasDisponibles
                        .Select(fecha => fecha.To<DisponibilidadGuiaFechaDto>())
                        .ToList(),

                    AusenciaGuiaDtos = guia.AusenciasProgramadas
                        .Select(ausencia => ausencia.To<AusenciaGuiaDto>())
                        .ToList()
                });
            }

            return new DisponibilidadGuiasDto
            {
                Desde = request.FechaDesde,
                Hasta = request.FechaHasta,
                GuiasDisponible = guiasDisponibles
            };
        }
    }
}
