using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Domain.RecursoMuseo.Enums.Enums;

namespace Application.MuseumResources.DataTransferObjects
{
    public class SalaDto
    {
        public string Nombre { get;private set; } = string.Empty;
        public EstadoSala Estado { get; private set; }
        public string CodigoSala { get; private set; } = string.Empty;
        public TipoSala TipoSala { get; private set; }
        public int Capacidad { get; private set; }
        public UbicacionSala Ubicacion { get; private set; }

    }
}
