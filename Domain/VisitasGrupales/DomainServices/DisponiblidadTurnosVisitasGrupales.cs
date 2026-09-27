using Domain.Common.Entities;
using Domain.Common.ValueObjets;
using Domain.RecursoMuseo.Entities.Guia;
using Domain.VisitasGrupales.Entities.GrupalGuiada;
using Domain.VisitasGrupales.Entities.GrupalGuiada.ReglasDisponibilidad;
using Domain.VisitasGrupales.ValueObjects;
using static Domain.VisitasGrupales.Enums.Enums;

namespace Domain.VisitasGrupales.DomainServices
{
    public class ServicioDisponibilidadTurnosVisitasGuiadas
        : IServicioDisponibilidadTurnosVisitasGuiadas
    {
        private readonly MotorDisponibilidadVisitasGuiadas _motor;

        public ServicioDisponibilidadTurnosVisitasGuiadas(
            MotorDisponibilidadVisitasGuiadas motor)
        {
            _motor = motor;
        }

        public Task<List<TurnoDisponible>> CalcularDisponibilidad(
            DateTime fechaDesde,
            DateTime fechaHasta,
            IReadOnlyCollection<Guia> guias,
            IReadOnlyCollection<VisitaGrupalGuiada> visitasGuiadas,
            ConfiguracionVisitasGrupalesGuiadas configuracion,
            CalendarioMuseo calendario)
        {
            if (configuracion == null)
                throw new ArgumentNullException(nameof(configuracion));

            if (calendario == null)
                throw new ArgumentNullException(nameof(calendario));

            var resultado = new List<TurnoDisponible>();

            for (
                DateTime fecha = fechaDesde;
                fecha <= fechaHasta;
                fecha = fecha.AddDays(1))
            {
                // 1. Verificar día disponible
                if (!configuracion.EsDiaDisponible(fecha.DayOfWeek))
                    continue;

                // 2. Verificar bloqueo de fecha
                if (!configuracion.EstaDisponibleEnFecha(fecha))
                {
                    foreach (var turno in configuracion.Turnos)
                    {
                        var horario = CrearTimeSlot(fecha, turno);

                        resultado.Add(
                            new TurnoDisponible(
                                horario,
                                0,
                                EstadoTurno.NoDisponible));
                    }

                    continue;
                }

                // 3. Procesar cada turno
                foreach (var turno in configuracion.Turnos)
                {
                    // Crear el TimeSlot correspondiente a ese turno y fecha
                    var horario = CrearTimeSlot(fecha, turno);

                    // 3.1. Verificar que el museo esté abierto
                    if (!calendario.EstaAbierto(
                        horario.Inicio,
                        horario.Fin))
                    {
                        resultado.Add(
                            new TurnoDisponible(
                                horario,
                                0,
                                EstadoTurno.NoDisponible));

                        continue;
                    }

                    // 3.2. Obtener las visitas que se solapan
                    var visitasDelTurno =
                        visitasGuiadas
                            .Where(v =>
                                v.Horario.SeSolapaCon(horario))
                            .ToList();

                    // 3.3. Obtener guías que pueden cubrir el turno
                    int cantidadGuias =
                        guias.Count(g =>
                            DisponibilidadGuiaFecha
                                .GuiaPuedeCubrirTurno(g, horario));

                    // 3.4. Calcular guías ocupados por visitas existentes
                    int guiasOcupados =
                        visitasDelTurno.Sum(v =>
                            CalcularGuiasNecesarios(
                                (int)v.CantidadPersonas,
                                configuracion.CapacidadPorGuia));

                    // 3.5. Guías libres
                    int guiasLibres =
                        Math.Max(
                            0,
                            cantidadGuias - guiasOcupados);

                    // 3.6. Crear contexto para el motor
                    var contexto =
                        new ContextoDisponibilidadVisitaGuiada(
                            configuracion,
                            turno,
                            visitasDelTurno,
                            guiasLibres);

                    // 3.7. EL MOTOR EVALÚA LAS REGLAS
                    var disponibilidad =
                        _motor.Evaluar(contexto);

                    // 3.8. Resultado del turno
                    resultado.Add(
                        new TurnoDisponible(
                            horario,
                            disponibilidad.CuposDisponibles,
                            disponibilidad.Disponible
                                ? EstadoTurno.Disponible
                                : EstadoTurno.NoDisponible));
                }
            }

            return Task.FromResult(resultado);
        }

        private static TimeSlot CrearTimeSlot(
            DateTime fecha,
            TurnoVisitaGuiada turno)
        {
            var inicio =
                fecha.Date.Add(
                    turno.HoraInicio.ToTimeSpan());

            var fin =
                fecha.Date.Add(
                    turno.HoraFin.ToTimeSpan());

            return new TimeSlot(inicio, fin);
        }

        private static int CalcularGuiasNecesarios(
            int cantidadPersonas,
            int capacidadPorGuia)
        {
            return (int)Math.Ceiling(
                (double)cantidadPersonas / capacidadPorGuia);
        }
    }
}