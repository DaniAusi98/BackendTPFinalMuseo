using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Domain.ActividadMuseo.Enums.Enums;
using static Domain.Common.Enums.Enums;

namespace Application.ActividadMuseo.UseCases.ActividadesMuseo.Commnads.BloqueoSala
{
    public class CrearBloqueoSalaCommand
    {
        public int SalaId { get; set; }

        public DateTime FechaDesde { get; set; }

        public DateTime FechaHasta { get; set; }

        public TipoBloqueoSala Motivo { get; set; }

        public string Observaciones { get; set; }
    }
}


// Hacer esto en handler 
/*var existeBloqueo = await _bloqueoSalaRepository
    .ExisteSuperposicionAsync(
        request.SalaId,
        request.FechaDesde,
        request.FechaHasta);

if (existeBloqueo)
    throw new BusinessException("La sala ya posee un bloqueo para ese período.");

var bloqueo = new BloqueoSala(
    request.SalaId,
    request.FechaDesde,
    request.FechaHasta,
    request.Motivo,
    request.Observaciones);
*/
