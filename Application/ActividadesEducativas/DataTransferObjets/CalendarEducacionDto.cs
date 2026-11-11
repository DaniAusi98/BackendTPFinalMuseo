using Application.VisitaGrupal.DataTransferObjets;

namespace Application.ActividadesEducativas.DataTransferObjets
{
    public class CalendarEducacionDto
    {
        
            public IEnumerable<VisitaGuiadaDto> VisitasGuiadas { get; set; }
            public IEnumerable<ActividadEducativaDto> ActividadesEducativas { get; set; }
        
    }
}
