using Application.Reportes.DataTransferObjets;
using Core.Application;
using System.ComponentModel.DataAnnotations;


namespace Application.Reportes.UseCases.Queries.ReporteVisitaGrupal
{
    public class ReporteGeneralVisitaGrupalQuery:IRequestQuery<ReporteVisitasGrupalesDto>
    {
        [Required]
        public DateTime Desde { get; set; }
        [Required]
        public DateTime Hasta { get; set; }

        public ReporteGeneralVisitaGrupalQuery()
        {

        }
            
    }
}
