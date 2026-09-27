using Application.ActividadesEducativas.DataTransferObjets;
using Core.Application;

namespace Application.ActividadesEducativas.UseCases.Queries
{
    public class DispActividadesEducativasQuery : IRequestQuery<DispActividadEducativaDto>
    {
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
        public DispActividadesEducativasQuery()
        {
        }

        public DispActividadesEducativasQuery(
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
