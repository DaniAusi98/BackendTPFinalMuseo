
using Application.ActividadMuseo.Repositories;
using Application.ApplicationMuseo.Constants;
using Application.Availability.Producers;
using Application.Common.ApplicationServices;
using Application.Eventos.Repositories;
using Application.Eventos.UseCases.Commands.UpdateEvento;
using Application.Exceptions;
using Application.MuseumResources.Repositories;
using Core.Application;
using Domain.ActividadMuseo.Entities;
using Domain.Common.ValueObjets;

namespace Application.Eventos.UseCases.UpdateEvento
{
    internal sealed class UpdateEventoHandler(
        IRepositorioEvento repositorioEvento,
        IStorageService storageService,
        IRepositorioSala repositorioSala,
        IRepositorioRecurso repositorioRecurso,
        IRepositorioPropuestaEvento repositorioPropuestaEvento,
        EventAvailabilityProducerService eventAvailabilityProducerService,
        IRepositorioActividadReprogramada repositorioActividadReprogramada
    ) : IRequestCommandHandler<UpdateEventoCommand, string>
    {
        private readonly IRepositorioEvento _repositorioEvento =
            repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));

        private readonly IRepositorioPropuestaEvento repositorioPropuestaEvento1 =
            repositorioPropuestaEvento ?? throw new ArgumentNullException(nameof(repositorioPropuestaEvento));

        private readonly IRepositorioRecurso _repositorioRecurso =
            repositorioRecurso ?? throw new ArgumentNullException(nameof(repositorioRecurso));

        private readonly IRepositorioSala _repositorioSala =
            repositorioSala ?? throw new ArgumentNullException(nameof(repositorioSala));

        private readonly IStorageService _storageService =
            storageService ?? throw new ArgumentNullException(nameof(storageService));

        private readonly EventAvailabilityProducerService _eventAvailabilityProducerService =
            eventAvailabilityProducerService;

        private readonly IRepositorioActividadReprogramada _repositorioActividadReprogramada =
            repositorioActividadReprogramada;

        public async Task<string> Handle(
            UpdateEventoCommand command,
            CancellationToken cancellationToken)
        {
            var evento = await _repositorioEvento
                .FindOneIncludeAsync(command.EventoId);

            if (evento is null)
                return null;

            var horarioAnterior = evento.Horario;

            // Comprobar si realmente cambió el horario.
            var nuevoHorario = new TimeSlot(
                command.Inicio,
                command.Fin);

            bool cambioHorario =
                horarioAnterior.Inicio != nuevoHorario.Inicio ||
                horarioAnterior.Fin != nuevoHorario.Fin;

            // 1. Guardar las URLs anteriores.
            var imagenesAnteriores = evento.UrlImagenes.ToList();

            // 2. Conservar las imágenes que envía el frontend.
            var imagenesConservadas = command.UrlImagenes?
             .Where(url => !string.IsNullOrWhiteSpace(url))
             .ToList() ?? [];

            // 3. Guardar las imágenes nuevas.
            if (command.ImagenesNuevas is not null)
            {
                foreach (var imagen in command.ImagenesNuevas)
                {
                    if (imagen.Contenido.Length == 0)
                        continue;

                    var url = await _storageService.GuardarArchivoAsync(
                        imagen,
                        "eventos",
                        cancellationToken);

                    if (!string.IsNullOrWhiteSpace(url))
                        imagenesConservadas.Add(url);
                }
            }

            // 4. Obtener las salas y recursos seleccionados.
            var salas = await _repositorioSala
                .ObtenerSalasporIdsAsync(command.SalasIds);

            var recursos = await _repositorioRecurso
                .ObtenerRecursosporIdsAsync(
                    command.Recursos.Select(r => r.RecursoId));

            var recursosUnicos = command.Recursos
                .GroupBy(r => r.RecursoId)
                .Select(g => g.Last())
                .ToList();

            var recursosAsignados = recursosUnicos
                .Select(r => new RecursoAsignado(
                    r.RecursoId,
                    r.CantidadAsignada,
                    nuevoHorario.Inicio,
                    nuevoHorario.Fin))
                .ToList();

            try
            {
                // 5. Actualizar la entidad.
                evento.Actualizar(
                    nombreyApellidoSolicitante: command.NombreyApellidoSolicitante,
                    telefonoSolicitante: new Telefono(command.TelefonoSolicitante),
                    emailSolicitante: new Email(command.EmailSolicitante),
                    tipoEvento: command.TipoEvento,
                    tituloEvento: command.TituloEvento,
                    salas: salas,
                    requiereDifusion: command.RequiereDifusion,
                    solicitaFlyer: command.SolicitarAsistenciaDifusion,
                    nuevoHorario,
                    institucion: command.Institucion,
                    descripcionEvento: command.DescripcionEvento,
                    fundamentacionEvento: command.FundamentacionEvento,
                    tipoPublico: command.TipoPublico,
                    ConcurrenciaEstimada: command.ConcurrenciaEstimada,
                    recursos: recursosAsignados,
                    recurrenceRule: command.RRule);

                // 6. Comprobar disponibilidad excluyendo el evento original.
                var disponible = await _eventAvailabilityProducerService
                    .EstaDisponibleAsync(evento, evento.Id);

                if (!disponible)
                {
                    throw new BussinessException(
                        "El horario seleccionado no se encuentra disponible.");
                }

                // 7. Persistir los cambios del evento.
                _repositorioEvento.Update(evento.Id, evento);

                // 8. Registrar la reprogramación solamente si cambió el horario.
                if (cambioHorario)
                {
                    var registroReprogramacion = new ActividadReprogramada(
                        evento.Id,
                        horarioAnterior,
                        nuevoHorario,
                        command.MotivoReprogramacion);

                    await _repositorioActividadReprogramada
                        .AddAsync(registroReprogramacion);
                }

                // 9. Eliminar físicamente las imágenes que se quitaron.
                var imagenesEliminadas = imagenesAnteriores
                    .Except(imagenesConservadas)
                    .ToList();

                foreach (var url in imagenesEliminadas)
                {
                    _storageService.EliminarArchivo(url, "eventos");
                }

                return evento.Id.ToString();
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
