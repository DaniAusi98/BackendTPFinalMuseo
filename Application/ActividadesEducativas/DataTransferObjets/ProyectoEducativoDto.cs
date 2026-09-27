using Domain.ActividadesAreaEducacion.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ActividadesEducativas.DataTransferObjets
{
    public class ProyectoEducativoDto
    {
        public string Nombre { get;  set; }
        public DateTime FechaInicio { get;  set; }
        public DateTime FechaFin { get;  set; }
        public List<EquipoResponsableDto> EquipoTrabajoResponsable { get;  set; } = new();
        public string Publico { get;  set; }
    }

    public class EquipoResponsableDto
    {

        public string NombreCompleto { get; set; }
        public string InstitucionPerteneciente { get; set; }

        public string ProyectoId { get; set; }
    }
}
