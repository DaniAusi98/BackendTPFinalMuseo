namespace Application.VisitaGrupal.DataTransferObjets
{
    public class ConfiguracionVisitasAutoguiadasDto
    {
        public string Id { get; private set; }
        public List<BloqueoVisitaAutoguiadaDto> Bloqueos { get; set; } = new();
        public List<HorarioDisponibleDto> DiasDisponibles { get; private set; }
        public int CapacidadMaximaPorGrupo { get; private set; }
        public TimeSpan DuracionVisita { get; private set; }
        public TimeSpan IntervaloReservas { get; private set; }
        public int VisitasSimultaneasMaximas { get; private set; }


    }
}
