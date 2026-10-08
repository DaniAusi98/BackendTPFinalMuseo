using Application.VisitaGrupal.DataTransferObjets;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Core.Application.Mapping;
using Domain.VisitasGrupales.DomainServices;

namespace Application.VisitaGrupal.UseCases.Guias.Queries
{
    internal sealed class DisponibilidadGuiaHandler(IRepositorioGuia repositorioGuia,
        IRepositorioAusenciaGuia repositorioAusenciaGuia,
             IRepositorioConfiguracionVisitasGrupalesGuiadas repositorioConfiguracion,
            ActividadMuseo.Repositories.IRepositorioCalendarioMuseo repositorioCalendario,
            IDisponibilidadGuia disponibilidadGuia) : IRequestQueryHandler<DisponibilidadGuiaQuery, DisponibilidadGuiaDto>
    {
        private readonly IRepositorioGuia _repositorioGuia = repositorioGuia ?? throw new ArgumentNullException(nameof(repositorioGuia));
        private readonly IRepositorioConfiguracionVisitasGrupalesGuiadas _repositorioConfiguracion = repositorioConfiguracion;
        private readonly ActividadMuseo.Repositories.IRepositorioCalendarioMuseo _repositorioCalendario = repositorioCalendario;
        private readonly IDisponibilidadGuia _disponibilidadGuia = disponibilidadGuia ?? throw new ArgumentNullException(nameof(disponibilidadGuia));
        private readonly IRepositorioAusenciaGuia _repositorio = repositorioAusenciaGuia ?? throw new ArgumentNullException(nameof(repositorioAusenciaGuia));
        public async Task<DisponibilidadGuiaDto> Handle(DisponibilidadGuiaQuery request, CancellationToken cancellationToken)
        {
            var guia = await _repositorioGuia.ObtenerGuiaConDisponibilidadAsync(request.GuiaId);
            var configuracion =
                await _repositorioConfiguracion
                    .ObtenerConfiguracionActivaAsync();

            var calendario =
                await _repositorioCalendario
                    .ObtenerCalendarioActivoAsync();
            var fechasDisponibles = await _disponibilidadGuia.CalcularDisponibilidadGuia(request.FechaDesde, request.FechaHasta, guia, configuracion, calendario);
            var ausenciasGuia = await _repositorio.GetAllAsync(request.GuiaId, request.FechaDesde, request.FechaHasta);




            List<DisponibilidadGuiaFechaDto> fechasDisponiblesDtos = [.. fechasDisponibles.Select(fecha => fecha.To<DisponibilidadGuiaFechaDto>())];

            List<AusenciaGuiaDto> ausenciasDtos = [.. ausenciasGuia.ToList().Select(ausencia => ausencia.To<AusenciaGuiaDto>())];
            return new DisponibilidadGuiaDto
            {
                FechasDisponiblesDtos = fechasDisponiblesDtos,
                AusenciaGuiaDtos = ausenciasDtos
            };
        }


    }
}
