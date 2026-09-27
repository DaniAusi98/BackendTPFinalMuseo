using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.ValueObjets
{


    public sealed class Horario
    {
        public TimeOnly HoraInicio { get; private set; }
        public TimeOnly HoraFin { get; private set; }

        private Horario() { }
        

     
        public Horario(TimeOnly inicio, TimeOnly fin)
        {
            if (fin <= inicio)
                throw new ArgumentException(
                    "El horario de fin debe ser posterior al inicio");

            HoraInicio = inicio;
            HoraFin = fin;
        }
    }

}
