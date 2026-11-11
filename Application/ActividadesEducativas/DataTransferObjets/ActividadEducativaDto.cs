using Application.ActividadMuseo.DataTransferObjets;
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
        public List<ActividadSalaDto> Salas { get; set; }
        public List<RecursosAsignadosEventoDto> Recursos { get; set; }
        public ActividadHorarioDto TimeSlot { get; set; }


    }


}
