using Application.ApplicationMuseo.Constants;
using Application.Common.ApplicationServices;
using Application.Eventos.Repositories;
using Application.Exceptions;
using Application.MuseumResources.Repositories;
using Core.Application;
using Domain.Common.ValueObjets;
using Domain.Eventos.Entities;
using Domain.Eventos.ValueObjets;

namespace Application.Eventos.UseCases.Commands.CrearMuestra
{
    internal sealed class CrearMuestraHandler(
        IRepositorioMuestraTemporal repositorioMuestra,
        IRepositorioSala repositorioSala,
        IStorageService storageService) : IRequestCommandHandler<CrearMuestraCommand, string>
    {
        private readonly IRepositorioMuestraTemporal _repositorioMuestra = repositorioMuestra;
        private readonly IRepositorioSala _repositorioSala = repositorioSala;
        private readonly IStorageService _storageService = storageService;

        public async Task<string> Handle(
            CrearMuestraCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                TimeSlot horario = new(request.Inicio, request.Fin);

                LogisticaMuestraTemporal logistica = new(
                    request.LogisticaMuestraTemporal.FechaMontaje,
                    request.LogisticaMuestraTemporal.FechaMontajeHasta,
                    request.LogisticaMuestraTemporal.FechaDesmontaje,
                    request.LogisticaMuestraTemporal.FechaDesmontajeHasta
                );

                var salas = await _repositorioSala.ObtenerSalasporIdsAsync(
                    request.SalasIds);

                List<string> imagenesGuardadas = [];

                if (request.ImagenesNuevas is not null)
                {
                    foreach (var imagen in request.ImagenesNuevas)
                    {
                        var url = await _storageService.GuardarArchivoAsync(
                            imagen,
                            "muestra",
                            cancellationToken);

                        if (!string.IsNullOrWhiteSpace(url))
                        {
                            imagenesGuardadas.Add(url);
                        }
                    }
                }

                Telefono telefono = new(request.TelefonoResponsable);
                Email email = new(request.EmailResponsable);

                Email? emailInstitucion = string.IsNullOrWhiteSpace(request.EmailInstitucion)
                    ? null
                    : new Email(request.EmailInstitucion);

                MuestraTemporal muestra = new(
                    request.NombreResponsable,
                    telefono,
                    email,
                    request.TituloPropuesta,
                    logistica,
                    request.RequiereDifusion,
                    salas,
                    horario,
                    request.Publico,
                    request.DescripcionMuestra,
                    request.FundamentacionMuestra,
                    request.Institucion,
                    emailInstitucion,
                    imagenesGuardadas
                );

                await _repositorioMuestra.AddAsync(muestra);

                return muestra.Id;
            }
            catch (Exception ex)
            {
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException);
            }
        }
    }
}