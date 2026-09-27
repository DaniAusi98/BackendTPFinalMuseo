using System.Text.Json.Serialization;
using static Domain.ActividadMuseo.Enums.Enums;
using static Domain.VisitasGrupales.Enums.Enums;

namespace Application.Reportes.DataTransferObjets
{
    public class ReporteGridDto
    {
        public string Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EstadoActividad Estado { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EstadoConfirmacionVisita EstadoConfirmacion { get; set; }  
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TipoVisitaGrupal Tipo { get; set; }
        public int CantidadPersonas { get; set; }
        public string Institucion { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
        public string Provincia { get; set; } = string.Empty;
        public string Localidad { get; set; } = string.Empty;
        public DateTime? HoraInicio { get; set; }
        public DateTime? HoraFin { get; set; }
    }
    public enum TipoVisitaGrupal
    {
        Guiada,
        Autoguiada
    }

}
