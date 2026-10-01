using Core.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ActividadesEducativas.UseCases.Queries.SalasDispActEducativas
{
    public class ObtenerSalasParaActEducQuery: IRequestQuery<QueryResult<SalasActEducDto>>
    {

    }
    public class SalasActEducDto
    {
        public string Id { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string CodigoSala { get; set; } = string.Empty;
        public int Capacidad { get; set; }
        public string Ubicacion { get; set; } = string.Empty;
        public string TipoSala { get; set; } = string.Empty;
    }
}
/* public class ObtenerSalasParaEventoQuery : IRequestQuery<QueryResult<SalaDisponibleParaEventoDto>>
    {
    }

    public class SalaDisponibleParaEventoDto
    {
        public string Id { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string CodigoSala { get; set; } = string.Empty;
        public int Capacidad { get; set; }
        public string Ubicacion { get; set; } = string.Empty;
        public string TipoSala { get; set; } = string.Empty;
    }*/