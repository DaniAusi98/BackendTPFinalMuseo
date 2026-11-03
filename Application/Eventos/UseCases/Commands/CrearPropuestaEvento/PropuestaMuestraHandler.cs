
using Application.ApplicationMuseo.Constants;
using Application.Eventos.ApplicationServices;
using Application.Eventos.Repositories;
using Application.Exceptions;
using Application.MuseumResources.Repositories;
using Core.Application;
using Domain.Common.ValueObjets;
using Domain.Eventos.Entities;
using Domain.Eventos.ValueObjets;
using Domain.RecursoMuseo.Entities;
using Microsoft.Extensions.Logging;
using static Domain.Eventos.Enums.Enums;

namespace Application.Eventos.UseCases.Commands.CrearPropuestaEvento
{
    internal sealed class CrearPropuestaMuestraHandler(
        IRepositorioMuestraTemporal repositorioMuestraTemporal,
        IRepositorioPropuestaMuestra repositorioPropuestaMuestra,
        IRepositorioSala repositorioSala,
        IRepositorioRecurso repositorioRecurso,
        IEventoApplicationService eventoApplicationService,
        IEnvioFormularioEventoEmailService envioFormularioEventoEmailService,
        IGenerarUrlFormularioEvento generarUrlFormularioEvento,
        ILogger<CrearPropuestaMuestraHandler> logger
    ) : IRequestCommandHandler<PropuestaMuestrasTemporalesCommand, string>
    {
        private readonly IRepositorioMuestraTemporal _repositorioMuestraTemporal = repositorioMuestraTemporal;
        private readonly IRepositorioPropuestaMuestra _repositorioPropuesta = repositorioPropuestaMuestra;
        private readonly IRepositorioSala _repositorioSala =
            repositorioSala ?? throw new ArgumentNullException(nameof(repositorioSala));

        private readonly IRepositorioRecurso _repositorioRecurso =
            repositorioRecurso ?? throw new ArgumentNullException(nameof(repositorioRecurso));

        private readonly IEnvioFormularioEventoEmailService _envioFormularioEventoEmailService =
            envioFormularioEventoEmailService ?? throw new ArgumentNullException(nameof(envioFormularioEventoEmailService));

        private readonly IGenerarUrlFormularioEvento _generarUrlFormularioEvento =
            generarUrlFormularioEvento ?? throw new ArgumentNullException(nameof(generarUrlFormularioEvento));

        private readonly IEventoApplicationService _eventoApplicationService =
            eventoApplicationService ?? throw new ArgumentNullException(nameof(eventoApplicationService));

        private readonly ILogger<CrearPropuestaMuestraHandler> _logger =
            logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<string> Handle(
            PropuestaMuestrasTemporalesCommand request,
            CancellationToken cancellationToken)
        {
            TimeSlot horario = new(request.Inicio, request.Fin);

            List<Sala> salas =
                await _repositorioSala.ObtenerSalasporIdsAsync(request.SalasIds);
            LogisticaMuestraTemporal logisticaMuestraTemporal = new
                LogisticaMuestraTemporal(
                request.LogisticaMuestraTemporalDto.FechaMontaje,
                request.LogisticaMuestraTemporalDto.FechaMontajeHasta,
                request.LogisticaMuestraTemporalDto.FechaDesmontaje,
                request.LogisticaMuestraTemporalDto.FechaDesmontajeHasta);

            MuestraTemporal muestra = new(
                request.NombreApellidoResponsable,
                telefonoResponsable: new Telefono(request.Telefono),
                emailResponsable: new Email(request.EmailResponsable),
                request.TituloPropuesta,
                logisticaMuestraTemporal,
                requiereDifusion: false,
                salas,
                horario
                );

            PropuestaMuestra propuestaMuestra = new(muestra);



            try
            {
                // 1. Persistir el evento y la propuesta.
                await _repositorioMuestraTemporal.AddAsync(muestra);
                await _repositorioPropuesta.AddAsync(propuestaMuestra);

                // 2. Intentar enviar el formulario por correo.
                try
                {
                    string url = _generarUrlFormularioEvento
                        .GenerarUrlFormularioEvento(muestra.Id, TipoEvento.MuestraExposicionTemporal);

                    await _envioFormularioEventoEmailService
                        .SendEnvioFormularioEmailAsync(
                            request.EmailResponsable,
                            request.TituloPropuesta,
                            url);


                    // 3. Registrar el envío exitoso.
                    propuestaMuestra.MarcarFormularioEnviado();

                    _repositorioPropuesta.Update(propuestaMuestra.Id, propuestaMuestra);
                }
                catch (Exception ex)
                {
                    // El fallo del correo no cancela la creación.
                    _logger.LogError(
                        ex,
                        "No se pudo enviar el formulario de la propuesta {PropuestaId}.",
                        propuestaMuestra.Id);
                }

                // 4. La propuesta fue creada, aunque falle el correo.
                return propuestaMuestra.Id;
            }
            catch (Exception ex)
            {
                throw new BussinessException(
                    ApplicationConstants.PROCESS_EXECUTION_EXCEPTION,
                    ex);
            }
        }
    }
}
