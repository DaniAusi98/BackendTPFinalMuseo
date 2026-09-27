using Application.Reportes.DataTransferObjets;
using Core.Application;
using System.ComponentModel.DataAnnotations;


namespace Application.Reportes.UseCases.Queries.ReporteVisitaGuiada
{
    public class ReporteVisitaGuiadaQuery : IRequestQuery<ReporteVisitaGuiadaDto>
    {
        [Required]
        public DateTime Desde { get; set; }
        [Required]
        public DateTime Hasta { get; set; }

        public ReporteVisitaGuiadaQuery()
        {
                
        }
    }
}
