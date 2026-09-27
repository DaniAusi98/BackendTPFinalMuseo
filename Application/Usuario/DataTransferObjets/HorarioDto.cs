using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Usuario.DataTransferObjets
{
    public class HorarioDto
    {
        public DateOnly Fecha { get; set; }
        public TimeOnly Inicio { get; set; }
        public TimeOnly Fin { get; set; }
    }

}
