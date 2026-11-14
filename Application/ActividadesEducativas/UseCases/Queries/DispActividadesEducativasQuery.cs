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
        /// IDs de las salas donde se quiere realizar la actividad.
        /// </summary>
        public List<string> SalasIds { get; set; } = new();

        /// <summary>
        /// ID de la actividad educativa que se excluye del cálculo de disponibilidad.
        /// Se utiliza al reprogramar una actividad existente.
        /// </summary>
        public string? ActividadEducativaIdExcluir { get; set; }

        public DispActividadesEducativasQuery()
        {
        }

        public DispActividadesEducativasQuery(
            DateTime desde,
            DateTime hasta,
            List<string> salasIds,
            string? actividadEducativaIdExcluir = null)
        {
            Desde = desde;
            Hasta = hasta;
            SalasIds = salasIds ?? throw new ArgumentNullException(nameof(salasIds));
            ActividadEducativaIdExcluir = actividadEducativaIdExcluir;
        }
    }
}