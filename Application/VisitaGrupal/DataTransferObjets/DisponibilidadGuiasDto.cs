using Application.VisitaGrupal.DataTransferObjets.Guias;

namespace Application.VisitaGrupal.DataTransferObjets
{
    public class DisponibilidadGuiasDto
    {
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }
        public List<DisponibilidadGuiaDto> GuiasDisponible { get; set; }
    }
}
