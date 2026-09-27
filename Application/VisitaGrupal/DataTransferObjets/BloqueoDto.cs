

namespace Application.VisitaGrupal.DataTransferObjets
{
    public class BloqueoDto
    {
        public string Tipo { get; set; }
        public DateOnly FechaDesde { get; set; }
        public DateOnly FechaHasta { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
    }
}
