using Domain.Common.Entities;
using Domain.VisitasGrupales.Entities;

namespace Domain.VisitasGrupales.DomainServices
{
    public interface IServicioDisponibilidadSlotsAutoguiadas
    {
        Task<List<SlotDisponibleVisitaAutoguiada>> CalcularDisponibilidad(
            DateTime fechaDesde,
            DateTime fechaHasta,
            IReadOnlyCollection<VisitaGrupalAutoguiada> visitasAutoguiadas,
            ConfiguracionHorarioAutoguiada configuracion,
            CalendarioMuseo calendario);
    }
}