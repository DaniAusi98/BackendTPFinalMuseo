
using Domain.VisitasGrupales.ValueObjects;

namespace Domain.VisitasGrupales.Entities.GrupalGuiada
{
    public class ContextoDisponibilidadVisitaGuiada
    {
        public ConfiguracionVisitasGrupalesGuiadas Configuracion { get; }

        public HorarioDisponible Horario { get; }

        public IReadOnlyCollection<VisitaGrupalGuiada> VisitasExistentes { get; }

        // Guías libres que pueden cubrir el turno.
        public int GuiasDisponibles { get; }

        // Personas reservadas en las visitas que se solapan.
        public int PersonasReservadas =>
            VisitasExistentes.Sum(v => (int)v.CantidadPersonas);

        // Capacidad de personas que queda en el turno.
        public int CapacidadPersonasRestante =>
            Math.Max(
                0,
                Configuracion.CapacidadMaximaPorTurno
                    - PersonasReservadas);

        public ContextoDisponibilidadVisitaGuiada(
            ConfiguracionVisitasGrupalesGuiadas configuracion,
            HorarioDisponible horario,
            IReadOnlyCollection<VisitaGrupalGuiada> visitasExistentes,
            int guiasDisponibles)
        {
            Configuracion = configuracion
                ?? throw new ArgumentNullException(nameof(configuracion));

            Horario = horario
                ?? throw new ArgumentNullException(nameof(horario));

            VisitasExistentes = visitasExistentes
                ?? throw new ArgumentNullException(nameof(visitasExistentes));

            if (guiasDisponibles < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(guiasDisponibles));
            }

            GuiasDisponibles = guiasDisponibles;
        }
    }
}
