using Core.Application;
using System.ComponentModel.DataAnnotations;
using static Domain.VisitasGrupales.Entities.GrupalGuiada.Enums.Enums;

namespace Application.VisitaGrupal.UseCases.VisitaGuiadaUC.Commands.ReprogramarVisitaGrupal.ReprogramarVisitaGuiada
{
    public class ReprogramarCommand : IRequestCommand<string>
    {
        [Required]
        public string VisitaReprogramadaId { get; set; }
        [Required]
        public string UsuarioVisitanteId { get; set; }

        public string NombreSolicitante { get; set; }
        public string ApellidoSolicitante { get; set; }

        public string EmailSolicitante { get; set; }
        public string TelefonoSolicitante { get; set; }

        [Required]
        public string Institucion { get; set; }

        public NivelEducativo? NivelEducativo { get; set; }

        public string? AnioGrado { get; set; }

        [Required]
        public string EmailInstitucion { get; set; }

        [Required]
        public string TelefonoInstitucion { get; set; }

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

        public List<string> TematicasIds { get; set; } = [];
        [Required]
        public DateTime Inicio { get; set; }
        [Required]
        public DateTime Fin { get; set; }

        public ReprogramarCommand() { }

    }

}
