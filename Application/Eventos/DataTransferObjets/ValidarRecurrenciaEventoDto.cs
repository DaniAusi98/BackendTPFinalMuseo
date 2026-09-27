namespace Application.Eventos.DataTransferObjets
{
    public class ValidarRecurrenciaEventoDto
    {
        public bool IsValid { get; set; }

        public List<SlotErrorDto> Errores { get; set; } = [];
    }

    public class SlotErrorDto
    {
        public DateTime Inicio { get; set; }

        public DateTime Fin { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}