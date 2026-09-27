using Application.Availability.Models;
using Domain.ActividadMuseo.Constants;
using Domain.Eventos.Entities;

namespace Application.Availability.Rules
{
    /// <summary>
    /// Regla que impide que una actividad se programe si hay un evento existente en el Hall de Ingreso.
    /// 
    /// Lógica: Candidata es otra actividad (VisitaGrupal, etc.) que quiere programarse → verificar si hay
    /// un evento existente en el Hall que se solape en horario. Si lo hay, rechazar (excepto si candidata
    /// es Muestra fuera del Hall).
    /// 
    /// Excepciones permitidas:
    /// - Muestras (MuestraExposicionTemporal) que NO se realicen en el Hall de Ingreso.
    /// </summary>
    public class NoActividadConEventoEnHall : IAvailabilityRule
    {
        public Task<AvailabilityResult> CheckAsync(
            AvailabilityContext ctx,
            CandidateEntry entry)
        {
            var candidate = entry.Candidate;

            // Esta regla no aplica a eventos
            if (candidate is Evento)
            {
                return Task.FromResult(AvailabilityResult.Ok());
            }

            // Si la candidata es una excepción, permitir
           // if (EsExcepcion(candidate))
           // {
           //     return Task.FromResult(AvailabilityResult.Ok());
           // }

            // Buscar si hay un evento existente en el Hall que se solape en horario
            var eventoExistenteEnHall = ctx.ExistingActivities
                .Where(a => a.Actividad is Evento evento &&
                            evento.Salas.Any(s => s.CodigoSala == DomainConstants.CODIGO_HALLINGRESO_PRINCIPAL))
                .FirstOrDefault(a => a.TimeSlots.Any(ts => entry.TimeSlots.Any(ets => ts.SeSolapaCon(ets))));

            if (eventoExistenteEnHall is not null)
            {
                return Task.FromResult(
                    AvailabilityResult.Fail(
                        "No se puede programar esta actividad. Hay un evento en el Hall de Ingreso en el mismo horario."));
            }

            return Task.FromResult(AvailabilityResult.Ok());
        }

        /// <summary>
        /// Determina si una actividad está excluida de la regla de conflicto.
        /// Excepción: MuestraExposicionTemporal que NO utilice el Hall de Ingreso.
        /// </summary>
        private static bool EsExcepcion(object activity)
        {
            // TODO: Descomentar cuando MuestraExposicionTemporal esté implementada
            // if (activity is MuestraExposicionTemporal muestra)
            // {
            //     return !muestra.Salas.Any(s => s.CodigoSala == DomainConstants.CODIGO_HALLINGRESO_PRINCIPAL);
            // }

            return false;
        }
    }
}
