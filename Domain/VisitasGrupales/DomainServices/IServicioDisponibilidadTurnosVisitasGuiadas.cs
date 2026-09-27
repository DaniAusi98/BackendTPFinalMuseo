using Domain.Common.Entities;
using Domain.RecursoMuseo.Entities.Guia;
using Domain.VisitasGrupales.Entities.GrupalGuiada;

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
            CalendarioMuseo calendario
           );
    }
}
