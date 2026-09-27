using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Domain.Common.Enums.Enums;

namespace Application.Common.UseCases.ActividadesMuseo.Commnads.DiaCierreMuseo
{
    public class CrearDiaCierreMuseoCommand
    {
        public DateTime Fecha { get; set; }

        public DateTime FechaHasta { get; set; }

        public MotivoCierreMuseo Motivo { get; set; }

        public string Observaciones { get; set; }
    }
}
