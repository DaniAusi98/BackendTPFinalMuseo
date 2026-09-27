using Application.ApplicationMuseo.DataTransferObjects;

namespace Application.VisitaGrupal.DataTransferObjets
{
    public class SlotDisponibleDto
    {
        public TimeSlotDto HorarioSlot { get; set; } = default!;
        public int CapacidadMaximaSlot { get; set; }
        public int CuposGrupoDisponibles { get; set; }
        public int CapacidadMaximaPorGrupo { get; set; }
        public string EstadoSlot { get; set; } = default!;
    }
}