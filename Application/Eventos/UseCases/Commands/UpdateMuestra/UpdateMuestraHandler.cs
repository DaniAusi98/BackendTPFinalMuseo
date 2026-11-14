using Application.ActividadMuseo.Repositories;
using Application.ApplicationMuseo.Constants;
using Application.Availability.Producers;
using Application.Common.ApplicationServices;
using Application.Eventos.Repositories;
using Application.Exceptions;
using Application.MuseumResources.Repositories;
using Core.Application;
using Domain.ActividadMuseo.Entities;
using Domain.Common.ValueObjets;
using Domain.Eventos.ValueObjets;

namespace Application.Eventos.UseCases.Commands.UpdateMuestra
{
    internal sealed class UpdateMuestraHandler(
        IRepositorioMuestraTemporal repositorioMuestraTemporal,
        IStorageService storageService,
        IRepositorioSala repositorioSala,
        IRepositorioRecurso repositorioRecurso,
        IRepositorioPropuestaMuestra repositorioPropuestaMuestra,
        DisponibilidadMuestrasExposicionesService disponibilidadMuestras,
        IRepositorioActividadReprogramada repositorioActividadReprogramada

        ) : IRequestCommandHandler<UpdateMuestraCommand, string>
    {
        private readonly IRepositorioMuestraTemporal _repositorioMuestra = repositorioMuestraTemporal ?? throw new ArgumentNullException(nameof(repositorioMuestraTemporal));
        private readonly IRepositorioPropuestaMuestra _repositorioPropuestaMuestra = repositorioPropuestaMuestra ?? throw new ArgumentNullException(nameof(repositorioPropuestaMuestra));
        private readonly IRepositorioRecurso _repositorioRecurso = repositorioRecurso ?? throw new ArgumentNullException(nameof(repositorioRecurso));
        private readonly IRepositorioSala _repositorioSala = repositorioSala ?? throw new ArgumentNullException(nameof(repositorioSala));
        private readonly IStorageService _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
        private readonly DisponibilidadMuestrasExposicionesService _disponibilidadMuestras = disponibilidadMuestras ?? throw new ArgumentNullException(nameof(disponibilidadMuestras));
        private readonly IRepositorioActividadReprogramada _repositorioActividadReprogramada = repositorioActividadReprogramada;
        public async Task<string> Handle(UpdateMuestraCommand request, CancellationToken cancellationToken)
        {
            var muestra = await _repositorioMuestra.FindOneIncludeAsync(request.MuestraId);

            if (muestra is null)
                return null;

            var horarioAnterior = muestra.Horario;
            var nuevoHorario = new TimeSlot(
               request.Inicio,
               request.Fin);

            bool cambioHorario =
                horarioAnterior.Inicio != nuevoHorario.Inicio ||
                horarioAnterior.Fin != nuevoHorario.Fin;
            var imagenesAnteriores = muestra.UrlImagenesMuestra.ToList();

            // 2. Conservar las imágenes que envía el frontend.
            var imagenesConservadas = request.UrlImagenes?
            .Where(url => !string.IsNullOrWhiteSpace(url))
            .ToList() ?? [];
            // 3. Guardar las imágenes nuevas.
            if (request.ImagenesNuevas is not null)
            {
                foreach (var imagen in request.ImagenesNuevas)
                {
                    if (imagen.Contenido.Length == 0)
                        continue;

                    var url = await _storageService.GuardarArchivoAsync(
                        imagen,
                        "muestra",
                        cancellationToken);

                    if (!string.IsNullOrWhiteSpace(url))
                        imagenesConservadas.Add(url);

                }
            }
            // 4. Obtener las salas y recursos seleccionados.
            var salas = await _repositorioSala
                .ObtenerSalasporIdsAsync(request.SalasIds);

            Telefono telefonoResponsable = new Telefono(request.TelefonoResponsable);
            Email emailResponsable = new Email(request.EmailResponsable);
            LogisticaMuestraTemporal logistica = new(
                    request.LogisticaMuestraTemporal.FechaMontaje,
                    request.LogisticaMuestraTemporal.FechaMontajeHasta,
                    request.LogisticaMuestraTemporal.FechaDesmontaje,
                    request.LogisticaMuestraTemporal.FechaDesmontajeHasta
                );

            var disponible = await _disponibilidadMuestras.EstaDisponibleAsync(
                request.SalasIds,
                nuevoHorario,
                logistica,
                request.MuestraId);

            if (!disponible)
                throw new BussinessException(
                    ApplicationConstants.PROCESS_EXECUTION_EXCEPTION,
                    new Exception("Las sala no está disponible en las fechas seleccionadas."));
            try
            {

                muestra.ActualizarMuestra(
                    request.NombreResponsable,
                    telefonoResponsable,
                    emailResponsable,
                    request.TituloPropuesta,
                    logistica,
                    request.Publico,
                    request.RequiereDifusion,
                    request.SolicitarAsistenciaDifusion,
                    nuevoHorario,
                    salas,
                    request.DescripcionMuestra,
                    request.FundamentacionMuestra,
                    urlImagenes: imagenesConservadas);


                _repositorioMuestra.Update(muestra.Id, muestra);
                if (cambioHorario)
                {
                    var registroReprogramacion = new ActividadReprogramada(
                        muestra.Id,
                        horarioAnterior,
                        nuevoHorario);

                    await _repositorioActividadReprogramada
                        .AddAsync(registroReprogramacion);
                }

                // 9. Eliminar físicamente las imágenes que se quitaron.
                var imagenesEliminadas = imagenesAnteriores
                    .Except(imagenesConservadas)
                    .ToList();

                foreach (var url in imagenesEliminadas)
                {
                    _storageService.EliminarArchivo(url, "muestra");
                }




                return muestra.Id.ToString();


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
