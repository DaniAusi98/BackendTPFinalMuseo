using Application.Eventos.DataTransferObjets;
using Core.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Eventos.UseCases.Queries
{
    public class ValidarRecurrenciaEventoQuery(
        DateTime inicio,
        DateTime fin,
        List<string> salasIds,
        string rrule) : IRequestQuery<ValidarRecurrenciaEventoDto>
    {
        public DateTime Inicio { get; } = inicio;
        public DateTime Fin { get; } = fin;
        public List<string> SalasIds { get; } = salasIds;
        public string RRule { get; } = rrule;
    }
}
