using Application.Reportes.DataTransferObjets;
using Core.Application;
using System.ComponentModel.DataAnnotations;

namespace Application.Reportes.UseCases.Queries.ReporteVisitanteSala
{
    public class ReporteVisitanteSalaQuery:IRequestQuery<List<ReporteVisitantesporSalaDto>>
    {
        [Required]
        public DateTime FechaInicio { get; set; }
        [Required]
        public DateTime FechaFin { get; set; }
        public ReporteVisitanteSalaQuery()
        {

        }

    }
}
