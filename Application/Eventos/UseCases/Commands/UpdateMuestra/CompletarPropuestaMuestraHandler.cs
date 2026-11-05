using Application.Common.ApplicationServices;
using Application.Eventos.DataTransferObjets;
using Application.Eventos.Repositories;
using Application.MuseumResources.Repositories;
using Core.Application;
using Core.Application.Mapping;
using Domain.Common.ValueObjets;
using Domain.Eventos.ValueObjets;

namespace Application.Eventos.UseCases.Commands.UpdateMuestra
{
    internal sealed class CompletarPropuestaMuestraHandler(
        IRepositorioMuestraTemporal repositorioMuestraTemporal,
        IStorageService storageService,
        IRepositorioSala repositorioSala,
        IRepositorioRecurso repositorioRecurso,
        IRepositorioPropuestaMuestra repositorioPropuestaMuestra
        ) : IRequestCommandHandler<CompletarPropuestaMuestraCommand, MuestraDto>
    {
        private readonly IRepositorioMuestraTemporal _repositorioMuestra = repositorioMuestraTemporal ?? throw new ArgumentNullException(nameof(repositorioMuestraTemporal));
        private readonly IRepositorioPropuestaMuestra _repositorioPropuestaMuestra = repositorioPropuestaMuestra ?? throw new ArgumentNullException(nameof(repositorioPropuestaMuestra));
        private readonly IRepositorioRecurso _repositorioRecurso = repositorioRecurso ?? throw new ArgumentNullException(nameof(repositorioRecurso));
        private readonly IRepositorioSala _repositorioSala = repositorioSala ?? throw new ArgumentNullException(nameof(repositorioSala));
        private readonly IStorageService _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));

        public async Task<MuestraDto> Handle(CompletarPropuestaMuestraCommand request, CancellationToken cancellationToken)
        {
            var muestra = await _repositorioMuestra.FindOneIncludeAsync(request.MuestraId);

            if (muestra is null)
                return null;

            var imagenesAnteriores = muestra.UrlImagenesMuestra.ToList();

            var imagenesConservadas = request.UrlImagenes
                .Where(url => !string.IsNullOrWhiteSpace(url))
                .ToList();
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
            var salas = await _repositorioSala
               .ObtenerSalasporIdsAsync(request.SalasIds);
            var telefonoResponsable = new Telefono(request.TelefonoResponsable);
            var emailResponsable = new Email(request.EmailResponsable);

            Email? emailInstitucion = null;

            if (request.EmailInstitucion != null)
            {
                emailInstitucion = new Email(request.EmailInstitucion);

            }
            else
            {
                emailInstitucion = null;
            }
            LogisticaMuestraTemporal logistica = new(request.LogisticaMuestraTemporal.FechaMontaje,
                request.LogisticaMuestraTemporal.FechaMontajeHasta, request.LogisticaMuestraTemporal.FechaDesmontaje,
                request.LogisticaMuestraTemporal.FechaDesmontajeHasta);


            muestra.ActualizarMuestra(request.NombreResponsable, telefonoResponsable, emailResponsable,
                request.TituloPropuesta, logistica, request.Publico, request.RequiereDifusion, salas, request.DescripcionMuestra,
                request.FundamentacionMuestra, request.Institucion, emailInstitucion, imagenesConservadas);

            _repositorioMuestra.Update(muestra.Id, muestra);
            var imagenesEliminadas = imagenesAnteriores
                .Except(imagenesConservadas)
                .ToList();

            foreach (var url in imagenesEliminadas)
            {
                _storageService.EliminarArchivo(url, "muestra");
            }
            var propuesta = await _repositorioPropuestaMuestra
               .FindByMuestraIdAsync(request.MuestraId);

            if (propuesta is not null)
            {
                propuesta.MarcarFormularioCompletado();
                _repositorioPropuestaMuestra.Update(propuesta.Id, propuesta);
            }

            return muestra.To<MuestraDto>();
        }
    }
}
