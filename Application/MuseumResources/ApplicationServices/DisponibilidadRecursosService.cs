using Application.ActividadMuseo.Repositories;
using Application.Availability.ApplicationServices;
using Application.Availability.Factories;
using Application.MuseumResources.DataTransferObjects;
using Application.MuseumResources.Repositories;
using Domain.Common.ValueObjets;

namespace Application.MuseumResources.ApplicationServices
{
    public class DisponibilidadRecursosService(
        IRepositorioActividadMuseo repositorioActividadMuseo,
        ActivityAvailabilityFactory activityAvailabilityFactory,
        IRecurrenceEvaluator recurrenceEvaluator,
        IRepositorioRecurso repositorioRecurso
    ) : IDisponibilidadRecursosService
    {
        private readonly IRepositorioActividadMuseo _repositorioActividadMuseo =
            repositorioActividadMuseo
            ?? throw new ArgumentNullException(nameof(repositorioActividadMuseo));

        private readonly ActivityAvailabilityFactory _activityAvailabilityFactory =
            activityAvailabilityFactory
            ?? throw new ArgumentNullException(nameof(activityAvailabilityFactory));

        private readonly IRecurrenceEvaluator _recurrenceEvaluator =
            recurrenceEvaluator
            ?? throw new ArgumentNullException(nameof(recurrenceEvaluator));

        private readonly IRepositorioRecurso _repositorioRecurso =
            repositorioRecurso
            ?? throw new ArgumentNullException(nameof(repositorioRecurso));

        public async Task<List<DisponibilidadRecursoDto>> ConsultarAsync(
            DateTime inicio,
            DateTime fin,
            string? rrule,
            string? eventoIdExcluir)
        {
            DateTime windowStart = inicio;
            DateTime windowEnd = fin;

            int duracionMinutos = (int)(fin - inicio).TotalMinutes;

            // 1. Calcular la ventana de la recurrencia.
            if (!string.IsNullOrWhiteSpace(rrule))
            {
                windowEnd = _recurrenceEvaluator.GetWindowEnd(
                    rrule,
                    inicio,
                    duracionMinutos);
            }

            // 2. Crear los TimeSlots de la nueva actividad.
            List<TimeSlot> timeSlotsNuevaActividad;

            if (string.IsNullOrWhiteSpace(rrule))
            {
                timeSlotsNuevaActividad =
                [
                    new TimeSlot(inicio, fin)
                ];
            }
            else
            {
                timeSlotsNuevaActividad = _recurrenceEvaluator.ExpandRule(
                    rrule,
                    inicio,
                    duracionMinutos,
                    windowStart,
                    windowEnd);
            }

            // 3. Consultar actividades existentes.
            var actividades = await _repositorioActividadMuseo.FindAllAsync(
                windowStart,
                windowEnd);

            // 4. Excluir la actividad que se está reprogramando, si corresponde.
            if (!string.IsNullOrWhiteSpace(eventoIdExcluir))
            {
                actividades = actividades
                    .Where(actividad => actividad.Id != eventoIdExcluir)
                    .ToList();
            }

            // 5. Expandir y normalizar las actividades existentes.
            var actividadesExistentes = _activityAvailabilityFactory.Create(
                actividades,
                windowStart,
                windowEnd);

            // 6. Consultar recursos activos.
            var recursosActivos = await _repositorioRecurso.FindActivosAsync();

            // Disponibilidad mínima encontrada para cada recurso.
            var disponibilidad = recursosActivos.ToDictionary(
                recurso => recurso.Id,
                recurso => recurso.CantidadTotal);

            // 7. Revisar cada ocurrencia de la nueva actividad.
            foreach (var timeSlotNuevo in timeSlotsNuevaActividad)
            {
                var actividadesSolapadas = actividadesExistentes
                    .Where(actividad => actividad.TimeSlots.Any(
                        slot => slot.SeSolapaCon(timeSlotNuevo)))
                    .ToList();

                // Agrupar los recursos ocupados por las actividades solapadas.
                var ocupadosPorRecurso = actividadesSolapadas
                    .SelectMany(actividad => actividad.Actividad.Recursos)
                    .GroupBy(recurso => recurso.RecursoId)
                    .ToDictionary(
                        grupo => grupo.Key,
                        grupo => grupo.Sum(recurso => recurso.CantidadAsignada));

                // Calcular disponibilidad para cada recurso activo.
                foreach (var recurso in recursosActivos)
                {
                    ocupadosPorRecurso.TryGetValue(
                        recurso.Id,
                        out var cantidadOcupada);

                    var cantidadDisponible =
                        recurso.CantidadTotal - cantidadOcupada;

                    // Conservar la menor disponibilidad entre las ocurrencias.
                    disponibilidad[recurso.Id] = Math.Min(
                        disponibilidad[recurso.Id],
                        cantidadDisponible);
                }
            }

            // 8. Construir respuesta.
            return recursosActivos
                .Select(recurso => new DisponibilidadRecursoDto
                {
                    RecursoId = recurso.Id,
                    Nombre = recurso.NombreRecurso,
                    Tipo = recurso.TipoRecurso.ToString(),
                    CantidadDisponible = disponibilidad[recurso.Id]
                })
                .ToList();
        }
    }
}