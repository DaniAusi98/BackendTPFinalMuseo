
using Application.Eventos.DataTransferObjets;
using Core.Application;

namespace Application.Eventos.UseCases.Queries
{
    public class ObtenerDisponibilidadEventoQuery : IRequestQuery<EventoDisponibilidadDto>
    {
        public DateTime Desde { get; set; }

        public DateTime Hasta { get; set; }

        public List<string> SalasIds { get; set; } = new();

        // Se utiliza únicamente al reprogramar un evento existente.
        public string? EventoIdExcluir { get; set; }

        public ObtenerDisponibilidadEventoQuery()
        {
        }

        public ObtenerDisponibilidadEventoQuery(
            DateTime desde,
            DateTime hasta,
            List<string> salasIds,
            string? eventoIdExcluir = null)
        {
            Desde = desde;
            Hasta = hasta;
            SalasIds = salasIds ?? throw new ArgumentNullException(nameof(salasIds));
            EventoIdExcluir = eventoIdExcluir;
        }
    }
}
