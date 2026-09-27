using Application.ActividadMuseo.Repositories;
using Application.Availability;
using Application.Availability.ApplicationServices; // Tu interfaz de expansión
using Application.Availability.Factories;
using Application.Availability.Models;
using Application.Eventos.Repositories;
using Application.Exceptions;
using Application.MuseumResources.Repositories;
using Domain.Common.ValueObjets;
using Domain.Eventos.Entities;
using static Domain.ActividadMuseo.Enums.Enums;
using static Domain.Eventos.Enums.Enums;
using ActividadMuseoEntity = Domain.ActividadMuseo.Entities.ActividadMuseo;

namespace Application.Eventos.ApplicationServices
{
    public class EventoApplicationService(
        IRepositorioEvento repositorioEvento,
        ActivityAvailabilityFactory activityFactory,
        AvailabilityEngine engine,
        IRepositorioSala repositorioSala,
        IRepositorioActividadMuseo repositorioActividadMuseo,
        IRecurrenceEvaluator recurrenceEvaluator) : IEventoApplicationService
    {
        private readonly IRepositorioEvento _repositorioEvento = repositorioEvento;
        private readonly ActivityAvailabilityFactory _activityFactory = activityFactory;
        private readonly AvailabilityEngine _engine = engine;
        private readonly IRepositorioSala _repositorioSala = repositorioSala;
        private readonly IRepositorioActividadMuseo _repositorioActividadMuseo = repositorioActividadMuseo;
        private readonly IRecurrenceEvaluator _recurrenceEvaluator = recurrenceEvaluator; // <--- INYECTAMOS EL EVALUADOR

        public async Task ValidarDisponibilidadResguardoAsync(
            string rrulePropuesta,
            TimeSlot horarioBasePropuesto,
            List<string> salasIdsSolicitadas)
        {
            // 1. Establecer el rango de la ventana de control
            DateTime desde = horarioBasePropuesto.Inicio.Date;
            // Si es recurrente controlamos 1 año completo en el futuro, si no, solo el día del evento
            DateTime hasta = string.IsNullOrWhiteSpace(rrulePropuesta)
                ? horarioBasePropuesto.Fin.Date
                : horarioBasePropuesto.Inicio.Date.AddYears(1);

            // ============================================================
            // A) EXPANDIR LOS SLOTS REALES DEL NUEVO EVENTO PROPUESTO
            // ============================================================
            List<TimeSlot> slotsRealesDelCandidato = new List<TimeSlot>();

            if (!string.IsNullOrWhiteSpace(rrulePropuesta))
            {
                // Calculamos los minutos de duración del bloque (ej: 60 minutos)
                int duracionMinutos = (int)(horarioBasePropuesto.Fin - horarioBasePropuesto.Inicio).TotalMinutes;

                // Usamos tu adaptador de infraestructura para expandir en memoria toda la serie de la regla
                slotsRealesDelCandidato = _recurrenceEvaluator.ExpandRule(
                    rrulePropuesta,
                    horarioBasePropuesto.Inicio,
                    duracionMinutos,
                    desde,
                    hasta
                );
            }
            else
            {
                // Si no tiene recurrencia, el único slot real es el bloque base enviado
                slotsRealesDelCandidato.Add(horarioBasePropuesto);
            }

            // Si por algún motivo la regla matemática no generó slots en ese rango, salimos de inmediato
            if (!slotsRealesDelCandidato.Any()) return;

            // ============================================================
            // B) PREPARAR LAS ACTIVIDADES EXISTENTES DE LA BD
            // ============================================================
            var actividadesEnRango = await ObtenerActividadesConConflictoPotencial(desde, hasta);

            var existingActivities = _activityFactory.Create(
                actividadesEnRango.Cast<ActividadMuseoEntity>().ToList(),
                desde,
                hasta
            );

            // ============================================================
            // C) CREAR EL CANDIDATO Y EJECUTAR TU ENGINE
            // ============================================================
            var salas = await _repositorioSala.ObtenerSalasporIdsAsync(salasIdsSolicitadas);

            var eventoAConfirmar = new Evento(
                nombreyApellidoSolicitante: "Resguardo Concurrente",
                telefonoSolicitante: new Telefono("3511234567"),
                emailSolicitante: new Email("resguardo@museo.com"),
                institucion: "Validación Interna",
                tipoEvento: TipoEvento.Conferencia,
                tituloEvento: "Validación Resguardo",
                descripcionEvento: "",
                fundamentacionEvento: "",
                tipoPublico: new List<TipoPublico> { TipoPublico.General },
                concurrenciaEstimada: 1,
                horario: horarioBasePropuesto,
                salas: salas,
                requiereDifusion: false,
                solicitaFlyer: false,
                recurrenceRule: rrulePropuesta // Tu entidad almacena el string
            );

            // AQUÍ ESTÁ LA SOLUCIÓN CLAVE: Pasamos la lista expandida con el .AsReadOnly()
            var candidatoUnico = new CandidateEntry(
                Guid.NewGuid(),
                eventoAConfirmar,
                slotsRealesDelCandidato.AsReadOnly(), // <--- El Engine ahora recibe TODOS los slots de la recurrencia
                null,
                "EventoPropuesto");

            var candidatos = new List<CandidateEntry> { candidatoUnico };

            var contexto = new AvailabilityContext
            {
                Start = desde,
                End = hasta,
                ExistingActivities = existingActivities,
                Metadata = new Dictionary<string, object>
                {
                    { "tipoActividad", TipoActividad.Evento },
                    { "salasIds", salasIdsSolicitadas }
                }
            };

            // Ejecutamos tu Engine tradicional
            var resultados = await _engine.CheckManyAsync(contexto, candidatos);

            // 8. Lanzar error de negocio si el motor detectó un choque en cualquiera de las repeticiones
            if (resultados.TryGetValue(candidatoUnico.Id, out var resultadoValidacion))
            {
                if (!resultadoValidacion.IsOk)
                {
                    throw new BussinessException(
                        $"La disponibilidad de las salas cambió. No se pudo confirmar la reserva. Motivo: {resultadoValidacion.Message}");
                }
            }


        }
        private async Task<List<ActividadMuseoEntity>> ObtenerActividadesConConflictoPotencial(DateTime desde, DateTime hasta)
        {
            var actividadesDirectas = await _repositorioActividadMuseo.FindAllAsync(desde, hasta); return actividadesDirectas;
        }
    }
}
