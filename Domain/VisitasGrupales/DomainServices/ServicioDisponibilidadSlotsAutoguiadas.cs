using Domain.Common.Entities;
using Domain.Common.ValueObjets;
using Domain.VisitasGrupales.Entities;

using static Domain.VisitasGrupales.Enums.Enums;

namespace Domain.VisitasGrupales.DomainServices
{
    public class ServicioDisponibilidadSlotsAutoguiadas
        : IServicioDisponibilidadSlotsAutoguiadas
    {
        /// <summary>
        /// Calcula la disponibilidad de horarios de entrada
        /// para visitas autoguiadas.
        ///
        /// Cada TimeSlot representa la permanencia completa
        /// del grupo dentro del museo.
        ///
        /// Ejemplo:
        /// Duración visita: 1 hora
        /// Intervalo entre entradas: 30 minutos
        /// Máximo simultáneo: 2 grupos
        ///
        /// Resultado:
        /// - 09:00-10:00 (Grupo 1 entra)
        /// - 09:30-10:30 (Grupo 2 entra, solapa con Grupo 1)
        /// - 10:00-11:00 (Grupo 3 entra)
        /// </summary>
        public Task<List<SlotDisponibleVisitaAutoguiada>>
            CalcularDisponibilidad(
                DateTime fechaDesde,
                DateTime fechaHasta,
                IReadOnlyCollection<VisitaGrupalAutoguiada> visitasAutoguiadas,
                ConfiguracionHorarioAutoguiada configuracion,
                CalendarioMuseo calendario)
        {
            if (configuracion == null)
                throw new ArgumentNullException(nameof(configuracion));

            if (calendario == null)
                throw new ArgumentNullException(nameof(calendario));

            if (visitasAutoguiadas == null)
                throw new ArgumentNullException(
                    nameof(visitasAutoguiadas));

            var resultado =
                new List<SlotDisponibleVisitaAutoguiada>();

            for (
                DateTime fecha = fechaDesde.Date;
                fecha <= fechaHasta.Date;
                fecha = fecha.AddDays(1))
            {
                var fechaDateOnly =
                    DateOnly.FromDateTime(fecha);

                // 1. Verificar si el día está configurado
                //    como día laboral para autoguiadas.
                if (!configuracion.DiasDisponibles
                    .EsDiaLaboral(fecha.DayOfWeek))
                {
                    continue;
                }

                // 2. Obtener los horarios posibles de entrada.
                var slots =
                    configuracion
                        .GenerarSlotsDisponibles(fechaDateOnly)
                        .ToList();

                // Si no existen slots, no hay nada que procesar.
                if (slots.Count == 0)
                    continue;

                // 3. Verificar si el museo está operativo.
                if (!calendario.EsDiaOperativo(fecha))
                {
                    foreach (var slot in slots)
                    {
                        resultado.Add(
                            new SlotDisponibleVisitaAutoguiada(
                                slot,
                                configuracion.CalcularCapacidadMaximaSimultanea(),
                                0,
                                configuracion.CapacidadMaximaPorGrupo,
                                EstadoTurno.NoDisponible));
                    }

                    continue;
                }

                // 4. Procesar cada horario posible de entrada.
                foreach (var slot in slots)
                {
                    // 4.1. Verificar que el horario completo
                    //      esté dentro del horario operativo.
                    if (!calendario.EstaAbierto(
                            slot.Inicio,
                            slot.Fin))
                    {
                        resultado.Add(
                            new SlotDisponibleVisitaAutoguiada(
                                slot,
                                configuracion
                                    .CalcularCapacidadMaximaSimultanea(),
                                0,
                                configuracion.CapacidadMaximaPorGrupo,
                                EstadoTurno.NoDisponible));

                        continue;
                    }

                    // 4.2. Determinar cuántas visitas existentes
                    //      se encuentran simultáneamente con
                    //      el nuevo grupo.
                    int visitasSimultaneas =
                        ContarVisitasSimultaneas(
                            slot,
                            visitasAutoguiadas);

                    // 4.3. Calcular cuántos grupos adicionales
                    //      pueden entrar.
                    int cuposDisponibles =
                        Math.Max(
                            0,
                            configuracion
                                .VisitasSimultaneasMaximas
                            - visitasSimultaneas);

                    // 4.4. Capacidad de personas disponible.
                    int capacidadDisponible =
                        cuposDisponibles *
                        configuracion.CapacidadMaximaPorGrupo;

                    // 4.5. Estado del horario.
                    EstadoTurno estado;

                    if (cuposDisponibles <= 0)
                    {
                        estado = EstadoTurno.Completo;
                    }
                    else
                    {
                        estado = EstadoTurno.Disponible;
                    }

                    // 4.6. Agregar resultado.
                    resultado.Add(
                        new SlotDisponibleVisitaAutoguiada(
                            slot,
                            configuracion.CalcularCapacidadMaximaSimultanea(),
                            cuposDisponibles,
                            configuracion.CapacidadMaximaPorGrupo,
                            estado));
                }
            }

            return Task.FromResult(resultado);
        }

        /// <summary>
        /// Cuenta las visitas existentes que se solapan
        /// con el período completo de permanencia del nuevo grupo.
        /// </summary>
        private static int ContarVisitasSimultaneas(
            TimeSlot slot,
            IReadOnlyCollection<VisitaGrupalAutoguiada> visitasAutoguiadas)
        {
            return visitasAutoguiadas.Count(
                visita =>
                    visita.Horario.SeSolapaCon(slot));
        }
    }
}