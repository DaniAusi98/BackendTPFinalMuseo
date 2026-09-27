using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ActividadMuseo.DataTransferObjets
{
    public class ActividadMuseoDto
    {
        public int Id { get; set; }
        public string CategoriaActividad { get; set; }
        public string TipoActividad { get; set; }
        public string Estado { get; set; }
        public int? CantidadPersonas { get; set; }
        public List<ActividadSalaDto> Salas { get; set; }
        public List<ActividadRecursoDto> Recursos { get; set; }
        public List<ActividadHorarioDto> TimeSlots { get; set; }
    }
}
