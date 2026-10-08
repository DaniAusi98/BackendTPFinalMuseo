namespace Application.VisitaGrupal.DataTransferObjets
{
    public class AusenciaGuiaDto
    {
        public string GuiaId { get; private set; }
        public DateTime FechaDesde { get; private set; }
        public DateTime FechaHasta { get; private set; }
        public string Motivo { get; private set; }
    }
}
