using Application.ActividadMuseo.Repositories;
using Application.ApplicationMuseo.Constants;
using Application.Availability.Producers;
using Application.Exceptions;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Domain.ActividadMuseo.Entities;
using Domain.Common.ValueObjets;

namespace Application.VisitaGrupal.UseCases.Commands.ReprogramarVisitaGrupal.ReprogramarAutoguiada
{
    internal sealed class ReprogramarAutoguiadaRecepcionCommandHandler(
        IRepositorioVisitaGrupalAutoguiada repositorioVisitaGrupalAutoguiada,
        IRepositorioTematicas repositorioTematicas,
        IRepositorioActividadReprogramada repositorioActividadReprogramada,
        SelfGuidedAvailabilityProducerService availabilityProducer
    ) : IRequestCommandHandler<ReprogramarAutoguiadaRecepcionCommand, string>
    {
        private readonly IRepositorioVisitaGrupalAutoguiada _repositorioVisitaGrupalAutoguiada =
            repositorioVisitaGrupalAutoguiada;

        private readonly IRepositorioTematicas _repositorioTematicas =
            repositorioTematicas;

        private readonly IRepositorioActividadReprogramada _repositorioActividadReprogramada =
            repositorioActividadReprogramada;

        private readonly SelfGuidedAvailabilityProducerService _availabilityProducer =
            availabilityProducer;

        public async Task<string> Handle(
            ReprogramarAutoguiadaRecepcionCommand request,
            CancellationToken cancellationToken)
        {
            var visita = await _repositorioVisitaGrupalAutoguiada
                .FindOneAsync(request.VisitaId)
                ?? throw new Exception("Visita no encontrada.");

            var horarioAnterior = visita.Horario;

            var nuevoHorario = new TimeSlot(
                request.Inicio,
                request.Fin);

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

            // Actualizar los datos de la visita existente.
            visita.AsignarTimeSlots(nuevoHorario);

            visita.CambiarCantidadAsistentes(
                request.CantidadPersonas);

            visita.ActualizarSalas(salas);

            visita.ActualizarDatos(
                request.NombreSolicitante,
                request.ApellidoSolicitante,
                new Email(request.EmailSolicitante),
                new Telefono(request.TelefonoSolicitante),
                request.Institucion,
                new Email(request.EmailInstitucion),
                request.PaisInstitucion,
                request.ProvinciaInstitucion,
                request.LocalidadInstitucion,
                request.DiversidadFuncionalDescripcion,
                request.Observaciones,
                tematicas);

            // Comprobar disponibilidad excluyendo la visita original.
            var disponible = await _availabilityProducer
                .IsSlotAvailableAsync(visita);

            if (!disponible)
            {
                throw new BussinessException(
                    "El horario seleccionado no se encuentra disponible.");
            }

            try
            {
                _repositorioVisitaGrupalAutoguiada.Update(
                    request.VisitaId,
                    visita);

                var registroReprogramacion = new ActividadReprogramada(
                    visita.Id,
                    horarioAnterior,
                    nuevoHorario,
                    request.MotivoReprogramacion
                );

                var registroReprogramacionId =
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