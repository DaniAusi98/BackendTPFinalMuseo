namespace Application.VisitaGrupal.DataTransferObjets
{
    public class BloqueoVisitaAutoguiadaDto
    {
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public string Motivo { get; set; } = string.Empty;
    }
}
