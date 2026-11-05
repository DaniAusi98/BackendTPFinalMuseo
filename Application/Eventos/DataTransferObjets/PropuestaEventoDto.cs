namespace Application.Eventos.DataTransferObjets
{
    public class PropuestaEventoDto
    {
        public string EventoId { get; private set; }
        public EventoDto Evento { get; private set; }

        public bool FormularioCompletado { get; private set; }
        public bool FormularioEnviado { get; private set; }
        public DateTime? FechaEnvioFormulario { get; private set; }
    }
}
