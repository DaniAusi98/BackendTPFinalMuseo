namespace Application.Eventos.DataTransferObjets
{
    public class TablaReporteEventoDto
    {
        public string Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string TipoEvento { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Institucion { get; set; } = string.Empty;
        public List<string> TipoPublico { get; set; } = [];
        public List<string> Salas { get; set; } = new List<string>();
        public string NombreSolicitante { get; set; } = string.Empty;
        public int CantidadEstimada { get; set; }
        public string Estado { get; set; } = string.Empty;

    }
}
