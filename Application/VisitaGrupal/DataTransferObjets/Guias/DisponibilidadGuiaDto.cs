namespace Application.VisitaGrupal.DataTransferObjets.Guias
{
    public class DisponibilidadGuiaDto
    {
        public string GuiaId { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;

        public List<GuiaHorarioDto> HorariosLaborales { get; set; } = [];

        public IEnumerable<DisponibilidadGuiaFechaDto> FechasDisponiblesDtos { get; set; } = [];
        public IEnumerable<AusenciaGuiaDto> AusenciaGuiaDtos { get; set; } = [];
    }
}