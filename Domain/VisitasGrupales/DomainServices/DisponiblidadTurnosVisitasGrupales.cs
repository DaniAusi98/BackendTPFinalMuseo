
using Domain.Common.Entities;
using Domain.Common.ValueObjets;
using Domain.VisitasGrupales.Entities.GrupalGuiada;
using Domain.VisitasGrupales.Entities.GrupalGuiada.Guia;
using Domain.VisitasGrupales.Entities.GrupalGuiada.ReglasDisponibilidad;
using Domain.VisitasGrupales.ValueObjects;
using static Domain.VisitasGrupales.Entities.GrupalGuiada.Enums.Enums;

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
            HorarioAperturaMuseo calendario)
        {
            ArgumentNullException.ThrowIfNull(configuracion);
            ArgumentNullException.ThrowIfNull(calendario);
            ArgumentNullException.ThrowIfNull(guias);
            ArgumentNullException.ThrowIfNull(visitasGuiadas);

            if (fechaDesde.Date > fechaHasta.Date)
                throw new ArgumentException(
                    "La fecha desde no puede ser posterior a la fecha hasta.");

            var resultado = new List<TurnoDisponible>();

            for (
                DateTime fecha = fechaDesde.Date;
                fecha <= fechaHasta.Date;
                fecha = fecha.AddDays(1))
            {
                // 1. Obtener los horarios configurados para ese día.
                var horariosDelDia = configuracion.DiasDisponibles
                    .Where(t => t.Dia == fecha.DayOfWeek)
                    .ToList();

                // Si no hay horarios configurados, no se generan turnos.
                if (!horariosDelDia.Any())
                    continue;

                // 2. Verificar si la fecha está bloqueada.
                if (!configuracion.EstaDisponibleEnFecha(fecha))
                {
                    foreach (var horarioDisponible in horariosDelDia)
                    {
                        var horario = CrearTimeSlot(
                            fecha,
                            horarioDisponible);

                        resultado.Add(
                            new TurnoDisponible(
                                horario,
                                0,
                                EstadoTurno.NoDisponible));
                    }

                    continue;
                }

                // 3. Evaluar cada horario configurado.
                foreach (var horarioDisponible in horariosDelDia)
                {
                    var horario = CrearTimeSlot(
                        fecha,
                        horarioDisponible);

                    // 3.1. Verificar que el museo esté abierto.
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

                    // 3.2. Buscar visitas que se solapan con el horario.
                    var visitasDelTurno = visitasGuiadas
                        .Where(v => v.Horario.SeSolapaCon(horario))
                        .ToList();

                    // 3.3. Contar guías capaces de cubrir el horario completo.
                    int cantidadGuias = guias.Count(g =>
                        ValidarDisponibilidadGuia
                            .GuiaPuedeCubrirTurno(g, horario));

                    // 3.4. Calcular los guías ocupados por las visitas existentes.
                    int guiasOcupados = visitasDelTurno.Sum(v =>
                        CalcularGuiasNecesarios(
                            (int)v.CantidadPersonas,
                            configuracion.CapacidadPorGuia));

                    // 3.5. Obtener los guías libres.
                    int guiasLibres = Math.Max(
                        0,
                        cantidadGuias - guiasOcupados);

                    // 3.6. Construir el contexto para el motor.
                    var contexto = new ContextoDisponibilidadVisitaGuiada(
                        configuracion,
                        horarioDisponible,
                        visitasDelTurno,
                        guiasLibres);

                    // 3.7. Evaluar las reglas de disponibilidad.
                    var disponibilidad = _motor.Evaluar(contexto);

                    // 3.8. Registrar el resultado.
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
            HorarioDisponible horarioDisponible)
        {
            var inicio = fecha.Date.Add(
                horarioDisponible.Desde.ToTimeSpan());

            var fin = fecha.Date.Add(
                horarioDisponible.Hasta.ToTimeSpan());

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
