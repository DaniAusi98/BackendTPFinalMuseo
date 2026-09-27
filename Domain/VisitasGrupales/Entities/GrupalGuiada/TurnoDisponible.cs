using Domain.Common.ValueObjets;
using static Domain.VisitasGrupales.Enums.Enums;

namespace Domain.VisitasGrupales.Entities.GrupalGuiada
{
    public record TurnoDisponible(
        TimeSlot horario,
        int cuposDisponibles,
        EstadoTurno estadoTurno)
    {
        public TimeSlot HorarioTurno { get; } = horario;

        public int CuposDisponibles { get; } = cuposDisponibles;

        public EstadoTurno EstadoTurno { get; } = estadoTurno;
    }
}