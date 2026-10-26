
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
            {
                throw new ArgumentException(
                    "La fecha desde no puede ser posterior a la fecha hasta.");
            }

            var resultado = new List<TurnoDisponible>();

            for (DateTime fecha = fechaDesde.Date;
                 fecha <= fechaHasta.Date;
                 fecha = fecha.AddDays(1))
            {
                var horariosDelDia = configuracion.DiasDisponibles
                    .Where(t => t.Dia == fecha.DayOfWeek)
                    .ToList();

                if (!horariosDelDia.Any())
                    continue;

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

                foreach (var horarioDisponible in horariosDelDia)
                {
                    var horario = CrearTimeSlot(
                        fecha,
                        horarioDisponible);

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

                    // Reservas que se solapan con este turno.
                    var visitasDelTurno = visitasGuiadas
                        .Where(v => v.Horario.SeSolapaCon(horario))
                        .ToList();

                    // Guías que pueden cubrir el turno.
                    int cantidadGuias = guias.Count(g =>
                        ValidarDisponibilidadGuia
                            .GuiaPuedeCubrirTurno(g, horario));

                    // Cada reserva consume los guías necesarios
                    // según su propia cantidad de personas.
                    int guiasOcupados = visitasDelTurno.Sum(v =>
                        CalcularGuiasNecesarios(
                            (int)v.CantidadPersonas,
                            configuracion.CapacidadPorGuia));

                    int guiasLibres = Math.Max(
                        0,
                        cantidadGuias - guiasOcupados);

                    var contexto = new ContextoDisponibilidadVisitaGuiada(
                        configuracion,
                        horarioDisponible,
                        visitasDelTurno,
                        guiasLibres);

                    var disponibilidad = _motor.Evaluar(contexto);

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
