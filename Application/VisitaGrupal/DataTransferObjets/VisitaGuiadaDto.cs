
using System;

namespace Application.VisitaGrupal.DataTransferObjets
{
    public class GuidedTourReservationDto
    {
        public string Id { get; set; }
        public string UsuarioVisitanteId { get; set; } = default!;
        public string Institucion { get; set; } = string.Empty;
        public string EmailInstitucion { get; set; } = string.Empty;
        public string TelefonoInstitucion { get; set; } = string.Empty;
        public string PaisInstitucion { get; set; } = string.Empty;
        public string ProvinciaInstitucion { get; set; } = string.Empty;
        public string CiudadInstitucion { get; set; } = string.Empty;
        public string? NivelCurso { get; set; }
        public string? AnioCurso { get; set; }
        public string DescripcionDiscapacidad { get; set; } = string.Empty;
        public string MotivoVisita { get; set; } = string.Empty;
        public string Observaciones { get; set; } = string.Empty;
        public List<TematicaVisitaDto> TematicasDto { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int CantidadPersonas { get; set; }
        public string EstadoConfirmacion { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }
}
