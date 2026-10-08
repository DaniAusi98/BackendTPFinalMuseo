using Domain.Common.Entities;
using Domain.Common.ValueObjets;
using Domain.RecursoMuseo.Entities.Guia;
using Domain.VisitasGrupales.Entities.GrupalGuiada;
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
            CalendarioMuseo calendario)
        {
            ArgumentNullException.ThrowIfNull(guia);
            ArgumentNullException.ThrowIfNull(configuracion);
            ArgumentNullException.ThrowIfNull(calendario);

            var resultado = new List<DisponibilidadGuiaFecha>();

            for (DateTime fecha = fechaDesde.Date; fecha <= fechaHasta.Date; fecha = fecha.AddDays(1))
            {
                // 1. Verificar si el día de la semana está habilitado en la configuración general
                if (!configuracion.EsDiaDisponible(fecha.DayOfWeek))
                    continue;

                // 2. Verificar si la fecha específica está bloqueada globalmente
                if (!configuracion.EstaDisponibleEnFecha(fecha))
                {
                    AgregarTurnosComoNoDisponibles(resultado, fecha, configuracion.Turnos);
                    continue;
                }

                // 3. Procesar cada turno configurado para el día
                foreach (var turno in configuracion.Turnos)
                {
                    var horario = CrearTimeSlot(fecha, turno);

                    var puede = ValidarDisponibilidadGuia
                        .GuiaPuedeCubrirTurno(guia, horario);

                    resultado.Add(
                        CrearDisponibilidad(horario, puede)
                    );
                    /*var horario = CrearTimeSlot(fecha, turno);

                    // 4. Validar que el museo esté abierto en este horario específico
                    if (!calendario.EstaAbierto(horario.Inicio, horario.Fin))
                    {
                        resultado.Add(CrearDisponibilidad(horario, disponible: false));
                        continue;
                    }

                    // 5. Validar restricciones contractuales o de agenda propias del guía
                    if (!ValidarDisponibilidadGuia.GuiaPuedeCubrirTurno(guia, horario))
                    {
                        resultado.Add(CrearDisponibilidad(horario, disponible: false));
                        continue;
                    }
                    resultado.Add(CrearDisponibilidad(horario, disponible: true));*/


                }
            }

            return Task.FromResult(resultado);
        }

        private static TimeSlot CrearTimeSlot(DateTime fecha, TurnoVisitaGuiada turno)
        {
            var inicio = fecha.Date.Add(turno.HoraInicio.ToTimeSpan());
            var fin = fecha.Date.Add(turno.HoraFin.ToTimeSpan());
            return new TimeSlot(inicio, fin);
        }

        private static DisponibilidadGuiaFecha CrearDisponibilidad(TimeSlot horario, bool disponible)
        {
            return new DisponibilidadGuiaFecha(
                DateOnly.FromDateTime(horario.Inicio),
                TimeOnly.FromDateTime(horario.Inicio),
                TimeOnly.FromDateTime(horario.Fin),
                disponible
            );
        }

        private static void AgregarTurnosComoNoDisponibles(
            List<DisponibilidadGuiaFecha> resultado,
            DateTime fecha,
            IEnumerable<TurnoVisitaGuiada> turnos)
        {
            foreach (var turno in turnos)
            {
                var horario = CrearTimeSlot(fecha, turno);
                resultado.Add(CrearDisponibilidad(horario, disponible: false));
            }
        }
    }
}
