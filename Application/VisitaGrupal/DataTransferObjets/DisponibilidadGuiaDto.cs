namespace Application.VisitaGrupal.DataTransferObjets
{
    public class DisponibilidadGuiaDto
    {
        public IEnumerable<DisponibilidadGuiaFechaDto> FechasDisponiblesDtos { get; set; }
        public IEnumerable<AusenciaGuiaDto> AusenciaGuiaDtos { get; set; }
    }
}
