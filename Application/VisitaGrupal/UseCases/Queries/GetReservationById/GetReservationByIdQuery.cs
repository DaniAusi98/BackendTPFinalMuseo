using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.VisitaGrupal.DataTransferObjets;

using Core.Application;

namespace Application.VisitaGrupal.UseCases.Queries.GetReservationById
{
    public class GetReservationByIdQuery:IRequestQuery<VisitaGuiadaDto>
    {
        public string ReservationId { get; set; }
        public GetReservationByIdQuery()
        {
                
        }
    }
}
