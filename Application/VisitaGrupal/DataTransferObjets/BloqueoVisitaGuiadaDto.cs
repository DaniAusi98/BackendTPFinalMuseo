using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.VisitaGrupal.DataTransferObjets
{
    public class BloqueoVisitaGuiadaDto
    {
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public string Motivo { get; set; } = string.Empty;
    }
}
