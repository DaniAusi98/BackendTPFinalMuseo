using System.Text.Json.Serialization;
using static Domain.Common.Enums.Enums;

namespace Application.ConfiguracionSistema.DataTransferObjets
{
    public class DiaCierreDto
    {
        public string Id { get; private set; }

        public DateTime Fecha { get; private set; }

        public DateTime? FechaHasta { get; private set; }


        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MotivoCierreMuseo Motivo { get; private set; }

        public string Observaciones { get; private set; }
    }
}