using Application.Eventos.DataTransferObjets;
using Core.Application;

namespace Application.Eventos.UseCases.Queries
{
    /// <summary>
    /// Query para obtener la disponibilidad de horas para un evento.
    /// Se consulta qué horas están disponibles en las salas especificadas
    /// para el rango de fechas solicitado.
    /// </summary>
    public class ObtenerDisponibilidadEventoQuery : IRequestQuery<EventoDisponibilidadDto>
    {
        /// <summary>
        /// Fecha de inicio del período a consultar
        /// </summary>
        public DateTime Desde { get; set; }

        /// <summary>
        /// Fecha de fin del período a consultar
        /// </summary>
        public DateTime Hasta { get; set; }

        /// <summary>
        /// IDs de las salas donde se quiere realizar el evento.
        /// La disponibilidad se calcula considerando conflictos en estas salas.
        /// </summary>
        public List<string> SalasIds { get; set; } = new();

        public ObtenerDisponibilidadEventoQuery()
        {
        }

        public ObtenerDisponibilidadEventoQuery(
            DateTime desde,
            DateTime hasta,
            List<string> salasIds)
        {
            Desde = desde;
            Hasta = hasta;
            SalasIds = salasIds ?? throw new ArgumentNullException(nameof(salasIds));
        }
    }
}
