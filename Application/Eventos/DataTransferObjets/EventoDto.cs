using Application.Common.DataTransferObjects;
using Application.MuseumResources.DataTransferObjects;
using static Domain.ActividadMuseo.Enums.Enums;
using static Domain.Eventos.Enums.Enums;

namespace Application.Eventos.DataTransferObjets
{
    public class EventoDto
    {
        public string Id { get; private set; }
        public string NombreyApellidoResponsable { get; private set; }
        public TelefonoDto TelefonoResponsable { get; private set; }
        public EmailDto EmailResponsable { get; private set; }
        public TimeSlotDto Horario { get; private set; }
        public string? Institucion { get; private set; }
        public string Titulo { get; private set; }
        public int? CantidadPersonas { get; private set; }
        public EstadoActividad EstadoActividad { get; private set; }
        public List<SalaDto> SalaDtos { get; private set; }
        public TipoEvento TipoEvento { get; private set; }
        public string DescripcionEvento { get; private set; }
        public string FundamentacionEvento { get; private set; }
        public string? TipoPublico { get; private set; }
        public int? CantidadEstimada { get; private set; }
        public bool RequiereDifusion { get; private set; }
        public List<string> UrlImagenes { get; private set; }
        public bool SolicitaFlyer { get; private set; }
        public string? RRule { get; private set; }
        public List<RecursosAsignadosEventoDto> Recursos { get; private set; } = [];
    }
}
