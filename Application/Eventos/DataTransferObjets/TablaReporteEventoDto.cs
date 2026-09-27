namespace Application.Eventos.DataTransferObjets
{
    public class TablaReporteEventoDto
    {
        public Guid Id { get; set; }
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
/*public string NombreyApellidoSolicitante { get; private set; }
        public Telefono TelefonoSolicitante { get; private set; }
        public Email EmailSolicitante { get; private set; }
        public string Institucion { get; private set; }
        public TipoEvento TipoEvento { get; private set; }
        public string TituloEvento { get; private set; }
        public string DescripcionEvento { get; private set; } = string.Empty;
        public string FundamentacionEvento { get; private set; } = string.Empty;
        public List<TipoPublico> TipoPublico { get; private set; }
        public int? CantidadEstimada { get; private set; }
        public bool RequiereDifusion { get; private set; }
        public List<string> UrlImagenes { get; private set; } = [];
        public bool SolicitaFlyer { get; private set; }*/