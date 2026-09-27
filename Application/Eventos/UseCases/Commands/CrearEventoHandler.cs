using Application.ApplicationMuseo.Constants;
using Application.Exceptions;
using Application.Eventos.Repositories;
using Application.MuseumResources.Repositories;
using Core.Application;
using Domain.ActividadMuseo.Entities;
using Domain.Common.ValueObjets;
using Domain.Eventos.Entities;
using Application.Eventos.ApplicationServices;

namespace Application.Eventos.UseCases.Commands
{
    internal sealed class CrearEventoHandler(
        IRepositorioEvento repositorioEvento,
        IRepositorioSala repositorioSala,
        IRepositorioRecurso repositorioRecurso,
        IEventoApplicationService eventoApplicationService) 
        : IRequestCommandHandler<CrearEventoCommand, string>
    {
        private readonly IRepositorioEvento _repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
        private readonly IRepositorioSala _repositorioSala = repositorioSala ?? throw new ArgumentNullException(nameof(repositorioSala));
        private readonly IRepositorioRecurso _repositorioRecurso = repositorioRecurso ?? throw new ArgumentNullException(nameof(repositorioRecurso));
        private readonly IEventoApplicationService _eventoApplicationService = eventoApplicationService ?? throw new ArgumentNullException(nameof(eventoApplicationService));

        public async Task<string> Handle(CrearEventoCommand request, CancellationToken cancellationToken)
        {
            var horario = new TimeSlot(request.Inicio, request.Fin);

            // 1. Validar la existencia de las salas
            var salas = await _repositorioSala.ObtenerSalasporIdsAsync(request.SalasIds);

            if (salas.Count != request.SalasIds.Count)
            {
                throw new BussinessException("Una o más salas no existen.");
            }

            // 2. Validar la existencia de los recursos
            var recursoIds = request.Recursos.Select(r => r.RecursoId).Distinct().ToList();
            var recursosDb = await _repositorioRecurso.FindAllAsync();
            var recursosSolicitados = recursosDb.Where(r => recursoIds.Contains(r.Id)).ToList();

            if (recursosSolicitados.Count != recursoIds.Count)
                throw new BussinessException("Uno o más recursos no existen.");

            var recursosAsignados = request.Recursos
                .Select(r => new RecursoAsignado(r.RecursoId, r.CantidadAsignada, request.Inicio, request.Fin))
                .ToList();

            // ============================================================
            // ÚLTIMA INSTANCIA: CONTROL DE RESGUARDO CON EL ENGINE
            // ============================================================
            // Se ejecuta ANTES de tocar la entidad o la base de datos.
            // Si la disponibilidad cambió, el Engine lanza un error de negocio aquí adentro.
            await _eventoApplicationService.ValidarDisponibilidadResguardoAsync(
                request.RRule,
                horario,
                request.SalasIds);

            // ============================================================
            // INSTANCIACIÓN DIRECTA DE LA ENTIDAD CON EL STRING RRULE
            // ============================================================
            var evento = new Evento(
                nombreyApellidoSolicitante: request.NombreyApellidoSolicitante,
                telefonoSolicitante: new Telefono(request.TelefonoSolicitante),
                emailSolicitante: new Email(request.EmailSolicitante),
                institucion: request.Institucion,
                tipoEvento: request.TipoEvento,
                tituloEvento: request.TituloEvento,
                descripcionEvento: request.DescripcionEvento,
                fundamentacionEvento: request.FundamentacionEvento,
                tipoPublico: request.TipoPublico,
                concurrenciaEstimada: request.ConcurrenciaEstimada,
                horario: horario,
                salas: salas,
                requiereDifusion: request.RequiereDifusion,
                solicitaFlyer: request.SolicitarAsistenciaDifusion,
                recursos: recursosAsignados,
                recurrenceRule: request.RRule
            );

            try
            {
                var createdId = await _repositorioEvento.AddAsync(evento);
                return createdId.ToString();
            }
            catch (Exception ex)
            {
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException);
            }
        }
    }
}
