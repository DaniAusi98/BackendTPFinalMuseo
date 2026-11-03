using Application.MuseumResources.DataTransferObjects;
using static Domain.Eventos.Enums.Enums;

namespace Application.Eventos.DataTransferObjets
{
    public class PropuestaEventoDto
    {
        public string TituloEvento { get; private set; }
        public TipoEvento TipoEvento { get; private set; }
        public string NombreyApellidoResponsable { get; private set; }
        public string TelefonoResponsable { get; private set; }
        public string EmailResponsable { get; private set; }
        public DateTime Inicio { get; private set; }
        public DateTime Fin { get; private set; }
        public List<SalaDto> Salas { get; private set; } = [];
        public List<RecursoAsignadoPropuestaDto> Recursos { get; private set; } = [];
        public bool EsRecurrencia => !string.IsNullOrWhiteSpace(RRule);
        public string? RRule { get; private set; }

        public bool FormularioEventoCompletado { get; private set; }
        public bool FormularioEnviado { get; private set; }
        public DateTime? FechaEnvioFormulario { get; private set; }
    }
}
