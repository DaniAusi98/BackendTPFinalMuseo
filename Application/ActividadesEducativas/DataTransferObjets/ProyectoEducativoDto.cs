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
        public string Id { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public List<EquipoResponsableDto> EquipoTrabajoResponsable { get; set; } = [];

        public string Publico { get; set; } = string.Empty;
    }

    public class EquipoResponsableDto
    {

        public string NombreCompleto { get; set; }
        public string InstitucionPerteneciente { get; set; }

        public string ProyectoId { get; set; }
    }
}
