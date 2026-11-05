using Application.Common.DataTransferObjects;
using Application.MuseumResources.DataTransferObjects;
using Domain.Eventos.ValueObjets;
using static Domain.ActividadMuseo.Enums.Enums;

namespace Application.Eventos.DataTransferObjets
{
    public class MuestraDto
    {
        public string MuestraId { get; private set; }
        public string NombreApellidoResponsable { get; private set; }
        public TelefonoDto TelefonoResponsable { get; private set; }
        public EmailDto EmailResponsable { get; private set; }
        public string? Institucion { get; private set; }
        public EmailDto? EmailInstitucion { get; private set; }
        public TimeSlotDto Horario { get; private set; }
        public string TituloPropuesta { get; private set; }
        public string DescripcionMuestra { get; private set; } = string.Empty;
        public string FundamentacionMuestra { get; private set; } = string.Empty;
        public LogisticaMuestraTemporal LogisticaMuestraTemporal { get; private set; }
        public List<SalaDto> SalaDtos { get; private set; }
        public string? Publico { get; private set; }
        public bool RequiereDifusion { get; private set; }
        public List<string> UrlImagenes { get; private set; }
        public bool SolicitaFlyer { get; private set; }
        public EstadoActividad EstadoActividad { get; private set; }

    }
    public record LogisticaMuestraTemporalDto
    {
        public DateTime FechaMontaje { get; set; }
        public DateTime FechaMontajeHasta { get; set; }
        public DateTime FechaDesmontaje { get; set; }
        public DateTime FechaDesmontajeHasta { get; set; }
    }
}
