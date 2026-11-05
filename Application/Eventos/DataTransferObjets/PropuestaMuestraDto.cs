namespace Application.Eventos.DataTransferObjets
{
    public class PropuestaMuestraDto
    {
        public string Id { get; private set; }
        public string MuestraTemporalId { get; private set; }
        public MuestraDto MuestraDto { get; private set; }
        public bool FormularioCompletado { get; private set; }
        public bool FormularioEnviado { get; private set; }
        public DateTime? FechaEnvioFormulario { get; private set; }
    }
}
