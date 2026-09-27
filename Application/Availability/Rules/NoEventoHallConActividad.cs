using Application.Availability.Models;
using Domain.ActividadMuseo.Constants;
using Domain.Eventos.Entities;

namespace Application.Availability.Rules
{
    /// <summary>
    /// Regla que impide que un evento se programe en el Hall de Ingreso si hay otra actividad en el mismo horario.
    /// 
    /// Lógica: Candidato es un evento que quiere usar el Hall  verificar si hay actividades existentes
    /// que se solapen en horario. Si las hay (excepto Muestras fuera del Hall), rechazar la candidata.
    /// 
    /// Excepciones permitidas:
    /// - Muestras (MuestraExposicionTemporal) que NO se realicen en el Hall de Ingreso.
    /// </summary>
    public class NoEventoHallConActividad : IAvailabilityRule
    {
        public Task<AvailabilityResult> CheckAsync(
            AvailabilityContext ctx,
            CandidateEntry entry)
        {
            var candidate = entry.Candidate;

            // Esta regla solo aplica a eventos
            if (candidate is not Evento evento)
            {
                return Task.FromResult(AvailabilityResult.Ok());
            }

            // Verificar si el evento utiliza el Hall de Ingreso Principal
            bool eventoEnHall = evento.Salas.Any(s => s.CodigoSala == DomainConstants.CODIGO_HALLINGRESO_PRINCIPAL);

            if (!eventoEnHall)
            {
                return Task.FromResult(AvailabilityResult.Ok());
            }

            // Buscar actividades que se solapen en horario (excepto excepciones)
            var actividadSolapada = ctx.ExistingActivities
                .Where(a => a.TimeSlots.Any(ts => entry.TimeSlots.Any(ets => ts.SeSolapaCon(ets))))
                .FirstOrDefault(a => !EsExcepcion(a));

            if (actividadSolapada is not null)
            {
                return Task.FromResult(
                    AvailabilityResult.Fail(
                        "El evento no puede programarse en el Hall de Ingreso porque hay otra actividad en el mismo horario."));
            }

            return Task.FromResult(AvailabilityResult.Ok());
        }

        /// <summary>
        /// Determina si una actividad está excluida de la regla de conflicto.
        /// Excepción: MuestraExposicionTemporal que NO utilice el Hall de Ingreso.
        /// </summary>
        private static bool EsExcepcion(ActivityAvailabilityEntry activity)
        {
            // TODO: Descomentar cuando MuestraExposicionTemporal esté implementada
            // if (activity.Actividad is MuestraExposicionTemporal muestra)
            // {
            //     return !muestra.Salas.Any(s => s.CodigoSala == DomainConstants.CODIGO_HALLINGRESO_PRINCIPAL);
            // }

            return false;
        }
    }
}