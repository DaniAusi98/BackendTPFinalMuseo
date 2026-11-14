
using Application.ActividadMuseo.Repositories;
using Application.ApplicationMuseo.Constants;
using Application.Availability.Producers;
using Application.Exceptions;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Domain.ActividadMuseo.Entities;
using Domain.Common.ValueObjets;

namespace Application.VisitaGrupal.UseCases.Commands.ReprogramarVisitaGrupal.ReprogramarVisitaGuiada
{
    internal sealed class ReprogramarVisitaGuiadaRecepcionCommandHandler(
        IRepositorioVisitaGuiada repositorioVisitaGuiada,
        IRepositorioTematicas repositorioTematicas,
        IRepositorioActividadReprogramada repositorioActividadReprogramada,
        GuidedAvailabilityProducerService availabilityProducer
    ) : IRequestCommandHandler<ReprogramarGuiadaRecepcionCommand, string>
    {
        private readonly IRepositorioVisitaGuiada _repositorioVisitaGuiada =
            repositorioVisitaGuiada;

        private readonly IRepositorioTematicas _repositorioTematicas =
            repositorioTematicas;

        private readonly IRepositorioActividadReprogramada _repositorioActividadReprogramada =
            repositorioActividadReprogramada;

        private readonly GuidedAvailabilityProducerService _availabilityProducer =
            availabilityProducer;

        public async Task<string> Handle(
            ReprogramarGuiadaRecepcionCommand request,
            CancellationToken cancellationToken)
        {
            var visita = await _repositorioVisitaGuiada
                .FindOneAsync(request.VisitaId)
                ?? throw new Exception("Visita no encontrada.");

            var horarioAnterior = visita.Horario;

            var nuevoHorario = new TimeSlot(
                request.Inicio,
                request.Fin);

            // Comprobar disponibilidad excluyendo la visita original.
            var disponible = await _availabilityProducer.IsTurnAvailableAsync(
                nuevoHorario,
                request.VisitaId,
                request.TematicasIds);

            if (!disponible)
            {
                throw new BussinessException(
                    "El horario seleccionado no se encuentra disponible.");
            }

            var tematicas = await _repositorioTematicas
                .GetByIdsAsync(request.TematicasIds);

            if (tematicas.Count != request.TematicasIds.Count)
            {
                throw new BussinessException(
                    "Una o más temáticas no existen.");
            }

            var salas = tematicas
                .SelectMany(t => t.Salas)
                .DistinctBy(s => s.Id)
                .ToList();

            try
            {
                // Actualizar la misma visita.
                visita.AsignarTimeSlots(nuevoHorario);

                visita.CambiarCantidadAsistentes(
                    request.CantidadPersonas);

                visita.ActualizarSalas(salas);

                visita.ActualizarDatosSolicitante(
                    request.NombreSolicitante,
                    request.ApellidoSolicitante,
                    new Email(request.EmailSolicitante),
                    new Telefono(request.TelefonoSolicitante));

                visita.ActualizarDatos(
                    request.Institucion,
                    new Email(request.EmailInstitucion),
                    new Telefono(request.TelefonoInstitucion),
                    request.PaisInstitucion,
                    request.ProvinciaInstitucion,
                    request.LocalidadInstitucion,
                    request.NivelEducativo,
                    request.AnioGrado,
                    request.DiversidadFuncionalDescripcion,
                    request.MotivoRelacionVisita,
                    request.Observaciones);

                visita.AsignarTematicas(tematicas);

                _repositorioVisitaGuiada.Update(
                    request.VisitaId,
                    visita);

                // Registrar la reprogramación.
                var registroReprogramacion = new ActividadReprogramada(
                    visita.Id,
                    horarioAnterior,
                    nuevoHorario,
                    request.MotivoReprogramacion);

                await _repositorioActividadReprogramada
                    .AddAsync(registroReprogramacion);

                return visita.Id.ToString();
            }
            catch (Exception ex)
            {
                throw new BussinessException(
                    ApplicationConstants.PROCESS_EXECUTION_EXCEPTION,
                    ex.InnerException ?? ex);
            }
        }
    }
}
