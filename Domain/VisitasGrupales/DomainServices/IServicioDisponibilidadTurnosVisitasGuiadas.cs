using Domain.Common.Entities;
using Domain.VisitasGrupales.Entities.GrupalGuiada;
using Domain.VisitasGrupales.Entities.GrupalGuiada.Guia;

namespace Domain.VisitasGrupales.DomainServices
{
    public interface IServicioDisponibilidadTurnosVisitasGuiadas
    {
       Task<List<TurnoDisponible>> CalcularDisponibilidad(
            DateTime fechaDesde,
            DateTime fechaHasta,
            IReadOnlyCollection<Guia> guias,
            IReadOnlyCollection<VisitaGrupalGuiada> visitasguiadas,
            ConfiguracionVisitasGrupalesGuiadas configuracion,
            HorarioAperturaMuseo calendario
           );
    }
}
