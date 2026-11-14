using Application.Common.DataTransferObjects;
using Application.MuseumResources.DataTransferObjects;

namespace Application.ActividadesEducativas.DataTransferObjets
{
    public class ActividadEducativaDto
    {
        public string Id { get; set; }
        public string Titulo { get; set; }
        public string TipoActividadEducativa { get; set; }
        public ProyectoEducativoDto ProyectoVinculado { get; private set; }
        public string InstitucionVinculada { get; private set; }
        public string PublicoObjetivo { get; private set; }
        public string Estado { get; set; }
        public int? CantidadPersonas { get; set; }
        public List<SalaDto> Salas { get; set; }
        public List<RecursosAsignadosEventoDto> Recursos { get; set; }
        public TimeSlotDto TimeSlot { get; set; }
        public string Observaciones { get; set; } = string.Empty;

        public bool RequiereDifusion { get; set; }

        public bool SolicitarAsistenciaDifusion { get; set; }

        public List<string> UrlImagenes { get; set; } = [];

        public string? RRule { get; set; }


    }


}
