
using Application.ApplicationMuseo.Constants;
using Application.Eventos.ApplicationServices;
using Application.Eventos.Repositories;
using Application.Exceptions;
using Application.MuseumResources.Repositories;
using Core.Application;
using Domain.ActividadMuseo.Entities;
using Domain.Common.ValueObjets;
using Domain.Eventos.Entities;
using Domain.RecursoMuseo.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Eventos.UseCases.Commands.CrearPropuestaEvento
{
    internal sealed class CrearPropuestaEventoHandler(
        IRepositorioEvento repositorioEvento,
        IRepositorioPropuestaEvento repositorioPropuestaEvento,
        IRepositorioSala repositorioSala,
        IRepositorioRecurso repositorioRecurso,
        IEventoApplicationService eventoApplicationService,
        IEnvioFormularioEventoEmailService envioFormularioEventoEmailService,
        IGenerarUrlFormularioEvento generarUrlFormularioEvento,
        ILogger<CrearPropuestaEventoHandler> logger
    ) : IRequestCommandHandler<CrearPropuestaEventoCommand, string>
    {
        private readonly IRepositorioEvento _repositorioEvento =
            repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));

        private readonly IRepositorioPropuestaEvento _repositorioPropuestaEvento =
            repositorioPropuestaEvento ?? throw new ArgumentNullException(nameof(repositorioPropuestaEvento));

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

        private readonly ILogger<CrearPropuestaEventoHandler> _logger =
            logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<string> Handle(
            CrearPropuestaEventoCommand request,
            CancellationToken cancellationToken)
        {
            TimeSlot horario = new(request.Inicio, request.Fin);

            List<Sala> salas =
                await _repositorioSala.ObtenerSalasporIdsAsync(request.SalasIds);

            var recursoIds = request.Recursos
                .Select(r => r.RecursoId)
                .Distinct()
                .ToList();

            var recursosDb = await _repositorioRecurso.FindAllAsync();

            var recursosSolicitados = recursosDb
                .Where(r => recursoIds.Contains(r.Id))
                .ToList();

            if (recursosSolicitados.Count != recursoIds.Count)
                throw new BussinessException("Uno o más recursos no existen.");

            var recursosAsignados = request.Recursos
                .Select(r => new RecursoAsignado(
                    r.RecursoId,
                    r.CantidadAsignada,
                    request.Inicio,
                    request.Fin))
                .ToList();

            // Validar disponibilidad antes de persistir.
            await _eventoApplicationService.ValidarDisponibilidadResguardoAsync(
                request.RRule,
                horario,
                request.SalasIds);

            Evento evento = new(
                request.NombreyApellidoResponsable,
                telefonoSolicitante: new Telefono(request.TelefonoResponsable),
                emailSolicitante: new Email(request.EmailResponsable),
                request.TipoEvento,
                request.TituloEvento,
                horario,
                salas,
                requiereDifusion: false,
                solicitaFlyer: false,
                recursos: recursosAsignados);

            PropuestaEvento propuesta = new(evento);

            try
            {
                // 1. Persistir el evento y la propuesta.
                await _repositorioEvento.AddAsync(evento);
                await _repositorioPropuestaEvento.AddAsync(propuesta);

                // 2. Intentar enviar el formulario por correo.
                try
                {
                    string url = _generarUrlFormularioEvento
                        .GenerarUrlFormularioEvento(evento.Id, evento.TipoEvento);

                    Console.WriteLine(url);
                    Console.WriteLine(url);
                    Console.WriteLine(url);
                    Console.WriteLine(url);
                    Console.WriteLine(url);
                    Console.WriteLine(url);

                    await _envioFormularioEventoEmailService
                        .SendEnvioFormularioEmailAsync(
                            request.EmailResponsable,
                            evento.Titulo,
                            url);


                    // 3. Registrar el envío exitoso.
                    propuesta.RegistrarEnvioFormulario();

                    _repositorioPropuestaEvento.Update(propuesta.Id, propuesta);
                }
                catch (Exception ex)
                {
                    // El fallo del correo no cancela la creación.
                    _logger.LogError(
                        ex,
                        "No se pudo enviar el formulario de la propuesta {PropuestaId}.",
                        propuesta.Id);
                }

                // 4. La propuesta fue creada, aunque falle el correo.
                return propuesta.Id;
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
