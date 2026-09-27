using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.ActividadMuseo.DataTransferObjets;

using Core.Application;

namespace Application.ActividadMuseo.UseCases.ActividadesMuseo.Queries
{
    public class GetAllActivitiesQuery (DateTime fechaConsultaDesde, DateTime fechaConsultaHasta):QueryRequest<QueryResult<ActividadMuseoDto>>
    {
        public DateTime FechaConsultaDesde { get; }= fechaConsultaDesde;
        public DateTime FechaConsultaHasta { get; }= fechaConsultaHasta;


    }
}
