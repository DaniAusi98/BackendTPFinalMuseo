using Domain.Common.Entities;
using Domain.Common.ValueObjets;
using Domain.VisitasGrupales.Entities.GrupalGuiada;
using Domain.VisitasGrupales.Entities.GrupalGuiada.Guia;
using Domain.VisitasGrupales.ValueObjects;

namespace Domain.VisitasGrupales.DomainServices
{
    public class DisponibilidadGuia : IDisponibilidadGuia
    {
        public Task<List<DisponibilidadGuiaFecha>> CalcularDisponibilidadGuia(
            DateTime fechaDesde,
            DateTime fechaHasta,
            Guia guia,
            ConfiguracionVisitasGrupalesGuiadas configuracion,
            HorarioAperturaMuseo calendario)
        {
            ArgumentNullException.ThrowIfNull(guia);
            ArgumentNullException.ThrowIfNull(configuracion);
            ArgumentNullException.ThrowIfNull(calendario);

            var resultado = new List<DisponibilidadGuiaFecha>();

            for (DateTime fecha = fechaDesde.Date;
                 fecha <= fechaHasta.Date;
                 fecha = fecha.AddDays(1))
            {
                // 1. Obtener solamente los horarios configurados
                // para el día de la semana correspondiente.
                var horariosDelDia = configuracion.DiasDisponibles
                    .Where(h => h.Dia == fecha.DayOfWeek)
                    .ToList();

                if (horariosDelDia.Count == 0)
                    continue;

                // 2. Filtrar los horarios que el guía puede cubrir
                // según su jornada laboral.
                var horariosDelGuia = horariosDelDia
                    .Where(h =>
                        ValidarDisponibilidadGuia.EstaDentroDelHorarioLaboral(
                            guia,
                            CrearTimeSlot(fecha, h)))
                    .ToList();

                if (horariosDelGuia.Count == 0)
                    continue;

                // 3. Si existe un bloqueo global de visitas guiadas,
                // informar estos horarios como no disponibles.
                if (!configuracion.EstaDisponibleEnFecha(fecha))
                {
                    AgregarTurnosComoNoDisponibles(
                        resultado,
                        fecha,
                        horariosDelGuia);

                    continue;
                }

                // 4. Evaluar cada horario del guía.
                foreach (var turno in horariosDelGuia)
                {
                    var horario = CrearTimeSlot(fecha, turno);

                    // El museo debe estar operativo durante
                    // todo el intervalo de la visita.
                    if (!calendario.EstaAbierto(
                        horario.Inicio,
                        horario.Fin))
                    {
                        resultado.Add(
                            CrearDisponibilidad(horario, false));

                        continue;
                    }

                    // Verificar estado del guía y sus ausencias.
                    bool disponible =
                        ValidarDisponibilidadGuia.GuiaPuedeCubrirTurno(
                            guia,
                            horario);

                    resultado.Add(
                        CrearDisponibilidad(horario, disponible));
                }
            }

            return Task.FromResult(resultado);
        }

        private static TimeSlot CrearTimeSlot(
            DateTime fecha,
            HorarioDisponible horario)
        {
            var inicio = fecha.Date.Add(
                horario.Desde.ToTimeSpan());

            var fin = fecha.Date.Add(
                horario.Hasta.ToTimeSpan());

            return new TimeSlot(inicio, fin);
        }

        private static DisponibilidadGuiaFecha CrearDisponibilidad(
            TimeSlot horario,
            bool disponible)
        {
            return new DisponibilidadGuiaFecha(
                DateOnly.FromDateTime(horario.Inicio),
                TimeOnly.FromDateTime(horario.Inicio),
                TimeOnly.FromDateTime(horario.Fin),
                disponible);
        }

        private static void AgregarTurnosComoNoDisponibles(
            List<DisponibilidadGuiaFecha> resultado,
            DateTime fecha,
            IEnumerable<HorarioDisponible> horarios)
        {
            foreach (var horario in horarios)
            {
                var timeSlot = CrearTimeSlot(fecha, horario);

                resultado.Add(
                    CrearDisponibilidad(timeSlot, false));
            }
        }
    }
}
