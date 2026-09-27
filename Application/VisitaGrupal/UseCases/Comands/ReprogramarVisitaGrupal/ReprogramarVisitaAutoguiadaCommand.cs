using Core.Application;
using System.ComponentModel.DataAnnotations;


namespace Application.VisitaGrupal.UseCases.Comands.ReprogramarVisitaGrupal
{
    public class ReprogramarVisitaAutoguiadaCommand : IRequestCommand<string>
    {
        [Required]
        public string VisitaReprogramadaId { get; set; }
        [Required]
        public string UsuarioVisitanteId { get; set; }
        [Required]
        public string Institucion { get; set; }
        [Required]
        public string EmailInstitucion { get; set; }
        [Required]
        public string PaisInstitucion { get; set; }
        [Required]
        public string ProvinciaInstitucion { get; set; }
       
        [Required]
        public string LocalidadInstitucion { get; set; }
        public string DiversidadFuncionalDescripcion { get; set; } = string.Empty;
        public string MotivoRelacionVisita { get; set; } = string.Empty;
        public string Observaciones { get; set; } = string.Empty;
        [Required]
        public int CantidadPersonas { get; set; }
        public List<string> TematicasIds { get; set; } = new List<string>();
        
        [Required]
        public DateTime Inicio { get; set; }
        
        [Required]
        public DateTime Fin { get; set; }
    }
}
