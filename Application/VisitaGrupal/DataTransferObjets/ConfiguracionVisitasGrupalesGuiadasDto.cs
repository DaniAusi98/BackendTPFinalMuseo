namespace Application.VisitaGrupal.DataTransferObjets
{
    public class ConfiguracionVisitasGrupalesGuiadasDto
    {
        public string Id { get; set; } = string.Empty;

        public int MaximoVisitasSimultaneas { get; set; }

        public int CapacidadPorGuia { get; set; }

        public int CapacidadMaximaPorTurno { get; set; }

        public List<HorarioDisponibleDto> DiasDisponibles { get; set; } = new();

        public List<BloqueoVisitaGuiadaDto> Bloqueos { get; set; } = new();
    }
}