using Application.Common.ApplicationServices;
using Application.Eventos.DataTransferObjets;
using Application.Eventos.Repositories;
using Application.Eventos.UseCases.Commands.UpdateEvento;
using Application.MuseumResources.Repositories;
using Core.Application;
using Core.Application.Mapping;
using Domain.ActividadMuseo.Entities;
using Domain.Common.ValueObjets;

namespace Application.Eventos.UseCases.UpdateEvento
{
    internal sealed class UpdateEventoHandler(
        IRepositorioEvento repositorioEvento,
        IStorageService storageService,
        IRepositorioSala repositorioSala,
        IRepositorioRecurso repositorioRecurso,
        IRepositorioPropuestaEvento repositorioPropuestaEvento)
        : IRequestCommandHandler<UpdateEventoCommand, EventoDto>
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

        public async Task<EventoDto> Handle(
            UpdateEventoCommand command,
            CancellationToken cancellationToken)
        {
            var evento = await _repositorioEvento
                .FindOneIncludeAsync(command.EventoId);

            if (evento is null)
                return null;

            // 1. Guardar las URLs anteriores para identificar las imágenes eliminadas.
            var imagenesAnteriores = evento.UrlImagenes.ToList();

            // 2. Conservar las imágenes que envía el frontend.
            var imagenesConservadas = command.UrlImagenes
                .Where(url => !string.IsNullOrWhiteSpace(url))
                .ToList();

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
                    evento.Horario.Inicio,
                    evento.Horario.Fin))
                .ToList();

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
                institucion: command.Institucion,
                descripcionEvento: command.DescripcionEvento,
                fundamentacionEvento: command.FundamentacionEvento,
                tipoPublico: command.TipoPublico,
                ConcurrenciaEstimada: command.ConcurrenciaEstimada,
                recursos: recursosAsignados,
                urlImagenes: imagenesConservadas,
                recurrenceRule: command.RRule);

            // 6. Persistir los cambios.
            _repositorioEvento.Update(evento.Id, evento);

            // 7. Eliminar físicamente las imágenes que se quitaron.
            var imagenesEliminadas = imagenesAnteriores
                .Except(imagenesConservadas)
                .ToList();

            foreach (var url in imagenesEliminadas)
            {
                _storageService.EliminarArchivo(url, "eventos");
            }

            var propuesta = await repositorioPropuestaEvento1
                .FindByEventoIdAsync(command.EventoId);

            propuesta?.RegistrarFormularioCompletado();

            repositorioPropuestaEvento1.Update(propuesta.Id, propuesta);

            // 8. Devolver el evento actualizado.
            return evento.To<EventoDto>();
        }
    }
}