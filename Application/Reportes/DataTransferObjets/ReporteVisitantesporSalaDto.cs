using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Reportes.DataTransferObjets
{
    public class ReporteVisitantesporSalaDto
    {

        public string SalaId { get; private set; }
        public string NombreSala { get; private set; }
        public int CantidadVisitantes { get; private set; }
        public ReporteVisitantesporSalaDto(
           string salaId,
           string nombreSala,
           int cantidadVisitantes)
        {
            SalaId = salaId;
            NombreSala = nombreSala;
            CantidadVisitantes = cantidadVisitantes;
        }

    }
}
