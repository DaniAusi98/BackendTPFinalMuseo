using Application.Availability.Models;
using Domain.ActividadMuseo.Entities;

namespace Application.Availability.Rules
{
    /// <summary>
    /// Regla general que impide que dos actividades usen la misma sala en el mismo horario.
    /// 
    /// Lógica: Candidata quiere usar una o más salas  verificar si hay actividades existentes
    /// que usen las mismas salas y se solapen en horario. Si las hay, rechazar.
    /// 
    /// Esta es la regla más general y aplica a TODAS las actividades sin excepciones.
    /// </summary>
    public class NoConflictoSala : IAvailabilityRule
    {
        public Task<AvailabilityResult> CheckAsync(
            AvailabilityContext ctx,
            CandidateEntry entry)
        {
            var candidate = entry.Candidate;

            // Si la candidata no tiene salas asignadas, no hay conflicto posible
            if (!candidate.Salas.Any())
            {
                return Task.FromResult(AvailabilityResult.Ok());
            }

            var salasCandidata = candidate.Salas.Select(s => s.Id).ToHashSet();

            // Buscar actividades que usen alguna de las mismas salas y se solapen en horario
            var conflicto = ctx.ExistingActivities
                .FirstOrDefault(a =>
                    a.Actividad.Salas.Any(s => salasCandidata.Contains(s.Id)) &&
                    a.TimeSlots.Any(ts => entry.TimeSlots.Any(ets => ts.SeSolapaCon(ets))));

            if (conflicto is not null)
            {
                var salasEnConflicto = string.Join(", ",
                    candidate.Salas
                        .Where(s => conflicto.Actividad.Salas.Any(cs => cs.Id == s.Id))
                        .Select(s => s.Nombre));

                return Task.FromResult(
                    AvailabilityResult.Fail(
                        $"No se puede programar la actividad. La sala(s) {salasEnConflicto} ya está(n) ocupada(s) en ese horario."));
            }

            return Task.FromResult(AvailabilityResult.Ok());
        }
    }
}