using Application.ActividadesEducativas.Repositories;
using Application.ActividadMuseo.Repositories;
using Application.ApplicationMuseo.Constants;
using Application.Availability.Producers;
using Application.Common.ApplicationServices;
using Application.Exceptions;
using Application.MuseumResources.Repositories;
using Core.Application;
using Domain.ActividadMuseo.Entities;
using Domain.Common.ValueObjets;

namespace Application.ActividadesEducativas.UseCases.Commands.UpdateActividaEducativa
{
    internal sealed class UpdateActividadEducativaHandler(
        IRepositorioActividadEducativa repositorioActividadEducativa,
        IStorageService storageService,
        IRepositorioSala repositorioSala,
        IRepositorioRecurso repositorioRecurso,
        IRepositorioActividadReprogramada repositorioActividadReprogramada,
        DispActEducativaProducerService dispActEducativaProducerService) : IRequestCommandHandler<UpdateActividadEducativaCommand, string>
    {
        private readonly IRepositorioActividadEducativa _repositorioActividadEducativa =
            repositorioActividadEducativa
            ?? throw new ArgumentNullException(nameof(repositorioActividadEducativa));
        private readonly IRepositorioRecurso _repositorioRecurso =
           repositorioRecurso ?? throw new ArgumentNullException(nameof(repositorioRecurso));

        private readonly IRepositorioSala _repositorioSala =
            repositorioSala ?? throw new ArgumentNullException(nameof(repositorioSala));

        private readonly IStorageService _storageService =
            storageService ?? throw new ArgumentNullException(nameof(storageService));
        private readonly IRepositorioActividadReprogramada _repositorioActividadReprogramada =
            repositorioActividadReprogramada;
        private readonly DispActEducativaProducerService _producerService = dispActEducativaProducerService
                ?? throw new ArgumentNullException(nameof(dispActEducativaProducerService));
        public async Task<string> Handle(UpdateActividadEducativaCommand command, CancellationToken cancellationToken)
        {
            var actividad = await _repositorioActividadEducativa.FindOneIncludeAsync(command.ActividadEducativaId);

            if (actividad is null)
                return null;

            var horarioAnterior = actividad.Horario;
            var nuevoHorario = new TimeSlot(
               command.Inicio,
               command.Fin);

            bool cambioHorario =
                horarioAnterior.Inicio != nuevoHorario.Inicio ||
                horarioAnterior.Fin != nuevoHorario.Fin;

            var imagenesAnteriores = actividad.UrlImagenes.ToList();

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
                        "actividadEducativa",
                        cancellationToken);

                    if (!string.IsNullOrWhiteSpace(url))
                        imagenesConservadas.Add(url);
                }
            }

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
                actividad.ActualizarActividadEducativa(command.Titulo,
                command.TipoActividadEducativa, command.ProyectoVinculadaId,
                command.InstitucionVinculada,
                command.PublicoObjetivo,
                command.Observaciones,
                nuevoHorario,
                salas,
                command.RequiereDifusion,
                command.SolicitarAsistenciaDifusion,
                command.CantidadAsistentes,
                imagenesConservadas,
                recursosAsignados,
                command.RRule
                );

                var disponible = await _producerService
                        .EstaDisponibleAsync(actividad, actividad.Id);

                if (!disponible)
                {
                    throw new BussinessException(
                        "El horario seleccionado no se encuentra disponible.");
                }



                _repositorioActividadEducativa.Update(actividad.Id, actividad);

                if (cambioHorario)
                {
                    var registroReprogramacion = new ActividadReprogramada(
                        actividad.Id,
                        horarioAnterior,
                        nuevoHorario
                        );

                    await _repositorioActividadReprogramada
                        .AddAsync(registroReprogramacion);


                }

                var imagenesEliminadas = imagenesAnteriores
                        .Except(imagenesConservadas)
                        .ToList();

                foreach (var url in imagenesEliminadas)
                {
                    _storageService.EliminarArchivo(url, "actividadEducativa");
                }

                return actividad.Id.ToString();
            }
            catch (Exception ex)
            {
                throw new BussinessException(
                    ApplicationConstants.PROCESS_EXECUTION_EXCEPTION,
                    ex.InnerException ?? ex);
            }











            throw new NotImplementedException();
        }
    }
}
