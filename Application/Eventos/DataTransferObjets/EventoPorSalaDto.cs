namespace Application.Eventos.DataTransferObjets
{
    public class EventoPorSalaDto
    {
        public List<string> Salas{ get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string TipoEvento { get; set; }
        public string Titulo { get; set; }
        public List<string> TipoPublico { get; set; }
        public int CantidadPersonas { get; set; }
        public List<string> Recursos { get; set; }
    }
}