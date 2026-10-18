using System;

namespace Application.VisitaGrupal.DataTransferObjets.Guias
{
    public class GuiaHorarioDto
    {
        public DayOfWeek DiaAsignado { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
    }
}
