using Application.ApplicationMuseo.DataTransferObjects;

namespace Application.VisitaGrupal.DataTransferObjets
{
    public class TurnoDisponibleDto
    {
       public TimeSlotDto HorarioTurno { get; set; } = default!;

        public int CuposDisponibles { get; set; }

        public string EstadoTurno { get; set; } = default!;

    }
}
