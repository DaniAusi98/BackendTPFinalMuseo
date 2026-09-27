using Application.Reportes.DataTransferObjets;
using Core.Application;
using System.ComponentModel.DataAnnotations;

namespace Application.Reportes.UseCases.Queries.ReporteVisitanteSala
{
    public class ReporteVisitaAutoguiadaQuery : IRequestQuery<ReporteVisitaAutoguiadaDto>
    {
        [Required]
        public DateTime Desde { get; set; }
        [Required]
        public DateTime Hasta { get; set; }
        public ReporteVisitaAutoguiadaQuery()
        {

        }
    }
}
