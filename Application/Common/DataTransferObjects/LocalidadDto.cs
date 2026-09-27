using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ApplicationMuseo.DataTransferObjects
{
    public class LocalidadDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int ProvinciaId { get; set; }
        public int DepartamentoId { get; set; }

    }
}
