using Application.VisitaGrupal.DataTransferObjets;

namespace Application.ActividadesEducativas.DataTransferObjets
{
    public class CalendarEducacionDto
    {
        
            public IEnumerable<GuidedTourReservationDto> VisitasGuiadas { get; set; }
            public IEnumerable<ActividadEducativaDto> ActividadesEducativas { get; set; }
        
    }
}
