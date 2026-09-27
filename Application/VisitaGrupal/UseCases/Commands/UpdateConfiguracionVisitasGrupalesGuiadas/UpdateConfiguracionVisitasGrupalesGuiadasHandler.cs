using Application.VisitaGrupal.Repositories;
using Application.ActividadMuseo.Repositories;
using Application.Exceptions;
using Core.Application;
using Domain.VisitasGrupales.ValueObjects;
using Domain.Common.ValueObjets;

namespace Application.VisitaGrupal.UseCases.Commands.UpdateConfiguracionVisitasGrupalesGuiadas
{
    internal sealed class UpdateConfiguracionVisitasGrupalesGuiadasHandler : IRequestCommandHandler<UpdateConfiguracionVisitasGrupalesGuiadasCommand>
    {
        private readonly IRepositorioConfiguracionVisitasGrupalesGuiadas _repositorio;
        private readonly IRepositorioCalendarioMuseo _repositorioCalendario;

        public UpdateConfiguracionVisitasGrupalesGuiadasHandler(
            IRepositorioConfiguracionVisitasGrupalesGuiadas repositorio,
            IRepositorioCalendarioMuseo repositorioCalendario)
        {
            _repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            _repositorioCalendario = repositorioCalendario ?? throw new ArgumentNullException(nameof(repositorioCalendario));
        }

        public async Task Handle(UpdateConfiguracionVisitasGrupalesGuiadasCommand request, CancellationToken cancellationToken)
        {
            var configuracion = await _repositorio.ObtenerConfiguracionActivaAsync()
                ?? throw new EntityDoesNotExistException("No existe una configuración activa para actualizar.");

            // Obtener el calendario del museo para validar
            var calendario = await _repositorioCalendario.ObtenerCalendarioActivoAsync()
                ?? throw new EntityDoesNotExistException("No se encontró el calendario del museo");

            configuracion.ActualizarCapacidad(
                request.MinGuiasParaCapacidadCompleta,
                request.CapacidadPorGuia,
                request.CapacidadMaximaPorTurno
            );

            var diasLaborales = new DiasLaboralesMuseo(request.DiasDisponibles);
            configuracion.ActualizarDiasDisponibles(diasLaborales, calendario);

            var turnos = request.Turnos
                .Select(t => new TurnoVisitaGuiada(t.HoraInicio, t.HoraFin))
                .ToList();
            configuracion.ActualizarTurnos(turnos);

            _repositorio.Update(configuracion.Id, configuracion);
        }
    }
}
