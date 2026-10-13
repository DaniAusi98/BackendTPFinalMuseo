using Domain.Common.ValueObjets;
using static Domain.VisitasGrupales.Entities.GrupalGuiada.Enums.Enums;

namespace Domain.VisitasGrupales.Entities.GrupalAutoguiada
{
    public record SlotDisponibleVisitaAutoguiada(TimeSlot horario, int capacidadMax, int cuposDisponibles,int capacidadMaxPorGrupo, EstadoTurno estadoSlot)
    {
        public TimeSlot HorarioSlot { get; } = horario;
        public int CapacidadMaximaSlot { get; } = capacidadMax;
        public int CuposGrupoDisponibles { get; } = cuposDisponibles;
        public int CapacidadMaximaPorGrupo { get; } = capacidadMaxPorGrupo;
        public EstadoTurno EstadoSlot { get; } = estadoSlot;
    }
}