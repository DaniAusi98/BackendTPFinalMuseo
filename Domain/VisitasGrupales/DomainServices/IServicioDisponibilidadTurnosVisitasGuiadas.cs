using Domain.Common.Entities;
using Domain.Common.ValueObjets;
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

        Task<bool> ValidarDisponibilidadTurno(
            TimeSlot horario,
            IReadOnlyCollection<Guia> guias,
            IReadOnlyCollection<VisitaGrupalGuiada> visitasGuiadas,
            ConfiguracionVisitasGrupalesGuiadas configuracion,
            HorarioAperturaMuseo calendario
            );
    }
}
