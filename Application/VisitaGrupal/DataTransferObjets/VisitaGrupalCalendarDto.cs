using System.Text.Json.Serialization;
namespace Application.VisitaGrupal.DataTransferObjets
{
    public class VisitaGrupalCalendarDto
    {
        public string Id { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TipoVisitaGrupal Tipo { get; set; }
        public string Pais { get; set; } = string.Empty;
        public string Provincia { get; set; } = string.Empty;
        public string Localidad { get; set; } = string.Empty;
        public int CantidadPersonas { get; set; }
        public string Institucion { get; set; } = string.Empty;
        public string DiversidadFuncional { get; set; } = string.Empty;
        public DateTime? HoraInicio { get; set; }
        public DateTime? HoraFin { get; set; }

        public enum TipoVisitaGrupal
        {
            Guiada,
            Autoguiada
        }
    }
}

