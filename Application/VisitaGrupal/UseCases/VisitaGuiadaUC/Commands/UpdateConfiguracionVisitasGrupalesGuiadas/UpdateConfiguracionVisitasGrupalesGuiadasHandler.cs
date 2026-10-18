
using Application.ConfiguracionSistema.Repositories;
using Application.Exceptions;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Domain.VisitasGrupales.ValueObjects;

namespace Application.VisitaGrupal.UseCases.VisitaGuiadaUC.Commands.UpdateConfiguracionVisitasGrupalesGuiadas
{
    internal sealed class UpdateConfiguracionVisitasGrupalesGuiadasHandler
        : IRequestCommandHandler<UpdateConfiguracionVisitasGrupalesGuiadasCommand>
    {
        private readonly IRepositorioConfiguracionVisitasGrupalesGuiadas _repositorio;
        private readonly IRepositorioCalendarioMuseo _repositorioCalendario;

        public UpdateConfiguracionVisitasGrupalesGuiadasHandler(
            IRepositorioConfiguracionVisitasGrupalesGuiadas repositorio,
            IRepositorioCalendarioMuseo repositorioCalendario)
        {
            _repositorio = repositorio
                ?? throw new ArgumentNullException(nameof(repositorio));

            _repositorioCalendario = repositorioCalendario
                ?? throw new ArgumentNullException(nameof(repositorioCalendario));
        }

        public async Task Handle(
            UpdateConfiguracionVisitasGrupalesGuiadasCommand request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var configuracion = await _repositorio.ObtenerConfiguracionActivaAsync()
                ?? throw new EntityDoesNotExistException(
                    "No existe una configuración activa para actualizar.");

            var calendario = await _repositorioCalendario.ObtenerCalendarioActivoAsync()
                ?? throw new EntityDoesNotExistException(
                    "No se encontró el calendario del museo.");

            var diasDisponibles = request.DiasDisponibles
                .Select(h => new HorarioDisponible(
                    h.Dia,
                    h.Desde,
                    h.Hasta))
                .ToList();

            // Validar y actualizar los horarios contra el calendario.
            configuracion.ActualizarDiasDisponibles(
                diasDisponibles,
                calendario);

            configuracion.ActualizarCapacidad(
                request.CapacidadPorGuia,
                request.CapacidadMaximaPorTurno,
                request.MaximoVisitasSimultaneas);

            _repositorio.Update(configuracion.Id, configuracion);
        }
    }
}
