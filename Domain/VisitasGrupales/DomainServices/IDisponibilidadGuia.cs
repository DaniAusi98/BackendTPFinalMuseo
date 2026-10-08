using Domain.Common.Entities;
using Domain.RecursoMuseo.Entities.Guia;
using Domain.VisitasGrupales.Entities.GrupalGuiada;

namespace Domain.VisitasGrupales.DomainServices
{
    public interface IDisponibilidadGuia
    {
        public Task<List<DisponibilidadGuiaFecha>> CalcularDisponibilidadGuia(
            DateTime fechaDesde,
            DateTime fechaHasta,
            Guia guia,
            ConfiguracionVisitasGrupalesGuiadas configuracion,
            CalendarioMuseo calendario);
    }
}
