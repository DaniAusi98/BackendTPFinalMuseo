using Application.ConfiguracionSistema.Repositories;
using Application.VisitaGrupal.DataTransferObjets.Guias;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Core.Application.Mapping;
using Domain.VisitasGrupales.DomainServices;

namespace Application.VisitaGrupal.UseCases.Guias.Queries.DisponibilidadGuia
{
    internal sealed class DisponGuiaUserIdHandler(
        IRepositorioGuia repositorioGuia,
        IRepositorioAusenciaGuia repositorioAusenciaGuia,
             IRepositorioConfiguracionVisitasGrupalesGuiadas repositorioConfiguracion,
            IRepositorioCalendarioMuseo repositorioCalendario,
            IDisponibilidadGuia disponibilidadGuia) : IRequestQueryHandler<DisponGuiaUserIdQuery, DisponibilidadGuiaDto>
    {
        private readonly IRepositorioGuia _repositorioGuia = repositorioGuia ?? throw new ArgumentNullException(nameof(repositorioGuia));
        private readonly IRepositorioConfiguracionVisitasGrupalesGuiadas _repositorioConfiguracion = repositorioConfiguracion;
        private readonly IRepositorioCalendarioMuseo _repositorioCalendario = repositorioCalendario;
        private readonly IDisponibilidadGuia _disponibilidadGuia = disponibilidadGuia ?? throw new ArgumentNullException(nameof(disponibilidadGuia));
        private readonly IRepositorioAusenciaGuia _repositorio = repositorioAusenciaGuia ?? throw new ArgumentNullException(nameof(repositorioAusenciaGuia));
        public async Task<DisponibilidadGuiaDto> Handle(DisponGuiaUserIdQuery request, CancellationToken cancellationToken)
        {
            var guia = await _repositorioGuia.ObtenerGuiaPersonalId(request.UserId);
            var configuracion =
                await _repositorioConfiguracion
                    .ObtenerConfiguracionActivaAsync();

            var calendario =
                await _repositorioCalendario
                    .ObtenerCalendarioActivoAsync();
            var fechasDisponibles = await _disponibilidadGuia.CalcularDisponibilidadGuia(request.FechaDesde, request.FechaHasta, guia, configuracion, calendario);
            var ausenciasGuia = await _repositorio.GetAllAsyncByGuideId(guia.Id, request.FechaDesde, request.FechaHasta);




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
