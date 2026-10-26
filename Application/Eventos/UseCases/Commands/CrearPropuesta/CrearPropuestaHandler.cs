using Application.Availability.ApplicationServices;
using Application.Common.Repositories;
using Application.Eventos.ApplicationServices;
using Application.Eventos.Repositories;
using Application.Exceptions;
using Application.MuseumResources.Repositories;
using Core.Application;
using Domain.Common.Entities;
using Domain.Common.ValueObjets;
using Domain.Eventos.Entities;
using Domain.RecursoMuseo.Entities;
using static Domain.Common.Enums.Enums;

namespace Application.Eventos.UseCases.Commands.CrearPropuesta
{
    internal class CrearPropuestaHandler(
        IRepositorioPropuestaEvento repositorioPropuestaEvento,
        IRepositorioPropuestaBloqueosSala repositorioPropuestaEventoBloqueoSala,
        IRepositorioSala repositorioSala,
        IRepositorioBloqueoSala repositorioBloqueoSala,
        IRepositorioRecurso repositorioRecurso,
        IRecurrenceEvaluator recurrenceEvaluator,
        IGenerarUrlFormularioEvento generarUrlFormularioEvento,
        IEnvioFormularioEventoEmailService envioFormularioEventoEmailService)
        : IRequestCommandHandler<CrearPropuestaCommand, string>
    {

        private readonly IRepositorioPropuestaEvento _repositorioPropuestaEvento =
            repositorioPropuestaEvento;

        private readonly IGenerarUrlFormularioEvento _generarUrlFormularioEvento = generarUrlFormularioEvento;
        private readonly IEnvioFormularioEventoEmailService _envioFormularioEventoEmailService = envioFormularioEventoEmailService;
        private readonly IRepositorioRecurso _repositorioRecurso = repositorioRecurso ?? throw new ArgumentNullException(nameof(repositorioRecurso));

        private readonly IRepositorioPropuestaBloqueosSala
            _repositorioPropuestaEventoBloqueoSala =
                repositorioPropuestaEventoBloqueoSala;

        private readonly IRepositorioSala _repositorioSala = repositorioSala;


        private readonly IRepositorioBloqueoSala _repositorioBloqueoSala =
            repositorioBloqueoSala;

        private readonly IRecurrenceEvaluator _recurrenceEvaluator =
            recurrenceEvaluator;

        public async Task<string> Handle(
            CrearPropuestaCommand request,
            CancellationToken cancellationToken)
        {
            // 1. Obtener las salas seleccionadas.
            List<Sala> salas =
                await _repositorioSala.ObtenerSalasporIdsAsync(
                    request.SalasIds);

            if (salas.Count == 0)
                throw new InvalidOperationException(
                    "La propuesta debe tener al menos una sala.");

            var recursoIds = request.Recursos.Select(r => r.RecursoId).Distinct().ToList();
            var recursosDb = await _repositorioRecurso.FindAllAsync();
            var recursosSolicitados = recursosDb.Where(r => recursoIds.Contains(r.Id)).ToList();

            if (recursosSolicitados.Count != recursoIds.Count)
                throw new BussinessException("Uno o más recursos no existen.");


            if (recursosSolicitados.Count != recursoIds.Count)
                throw new BussinessException("Uno o más recursos no existen.");

            var recursosAsignados = request.Recursos
                .Select(r => new PropuestasRecursos(r.RecursoId, r.CantidadAsignada, request.Fecha, request.FechaHasta))
                .ToList();

            // 2. Calcular la duración de una ocurrencia.
            int duracionMinutos = (int)(
                request.FechaHasta - request.Fecha).TotalMinutes;

            if (duracionMinutos <= 0)
                throw new InvalidOperationException(
                    "La fecha de fin debe ser posterior a la fecha de inicio.");

            // 3. Obtener los turnos que se deben bloquear.
            List<TimeSlot> ocurrencias;

            if (string.IsNullOrWhiteSpace(request.RRule))
            {
                ocurrencias =
                [
                    new TimeSlot(request.Fecha, request.FechaHasta)
                ];
            }
            else
            {
                // Ventana de un año desde la postulación.
                DateTime fechaPostulacion = DateTime.Now;
                DateTime finVentana = fechaPostulacion.AddYears(1);

                ocurrencias = _recurrenceEvaluator.ExpandRule(
                    request.RRule,
                    request.Fecha,
                    duracionMinutos,
                    fechaPostulacion,
                    finVentana);
            }

            // 4. Crear un bloqueo por cada turno y cada sala.
            List<BloqueoSala> bloqueosSala = [];

            foreach (TimeSlot ocurrencia in ocurrencias)
            {
                foreach (Sala sala in salas)
                {
                    var bloqueo = new BloqueoSala(
                        sala.Id,
                        ocurrencia.Inicio,
                        ocurrencia.Fin,
                        TipoBloqueoSala.Otro,
                        "Espacio Reservado");

                    bloqueosSala.Add(bloqueo);
                }
            }

            // 5. Guardar los bloqueos.
            if (bloqueosSala.Count > 0)
            {
                await _repositorioBloqueoSala.AddRangeAsync(
                    bloqueosSala);
            }

            // 6. Crear la propuesta.
            var propuesta = new PropuestaEvento(
                request.NombreApellidoResponsable,
                new Telefono(request.TelefonoResponsable),
                new Email(request.EmailResponsable),
                request.TituloPropuesta,
                request.TipoEvento,
                request.Fecha,
                request.FechaHasta,
                salas,
                recursosAsignados,
                request.RRule);

            // 7. Guardar la propuesta.
            await _repositorioPropuestaEvento.AddAsync(propuesta);

            // 8. Crear las relaciones entre propuesta y bloqueos.
            List<PropuestaEventoBloqueoSala> propuestaBloqueos =
                [.. bloqueosSala
                    .Select(bloqueo =>
                        new PropuestaEventoBloqueoSala(
                            propuesta.Id,
                            bloqueo.Id))];

            // 9. Guardar las relaciones.
            if (propuestaBloqueos.Count > 0)
            {
                await _repositorioPropuestaEventoBloqueoSala
                    .AddRangeAsync(propuestaBloqueos);
            }
            string urlFormulario = _generarUrlFormularioEvento.GenerarUrlFormularioEvento(
                propuesta.Id,
                propuesta.TipoEvento);
            await envioFormularioEventoEmailService
            .SendEnvioFormularioEmailAsync(
                request.EmailResponsable,
                propuesta.TituloPropuesta,
                urlFormulario);

            propuesta.MarcarFormularioEnviado();

            _repositorioPropuestaEvento.Update(propuesta.Id, propuesta);

            return propuesta.Id;
        }
    }
}