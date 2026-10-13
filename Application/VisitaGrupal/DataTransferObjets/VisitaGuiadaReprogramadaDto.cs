using Application.ApplicationMuseo.DataTransferObjects;
using static Domain.ActividadMuseo.Enums.Enums;
using static Domain.VisitasGrupales.Entities.GrupalGuiada.Enums.Enums;

namespace Application.VisitaGrupal.DataTransferObjets
{
    public class VisitaGuiadaReprogramadaDto
    {
        public string VisitaReprogramadaId { get; set; }
        public string NuevaVisitaId { get; set; }
        public TimeSlotDto HorarioReprogrado { get; set; }
        public string Institucion { get; set; }
        public NivelEducativo? NivelEducativo { get; set; }
        public string? AnioGrado { get; set; }
        public string PaisInstitucion { get; set; }
        public string ProvinciaInstitucion { get; set; }
        public string LocalidadInstitucion { get; set; }
        public EstadoActividad Estado { get; set; }
        public int? CantidadPersonas { get; set; }
        public TimeSlotDto NuevoHorario { get; set; }
    }
}
