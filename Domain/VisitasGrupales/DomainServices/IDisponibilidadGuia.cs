using Domain.Common.Entities;
using Domain.VisitasGrupales.Entities.GrupalGuiada;
using Domain.VisitasGrupales.Entities.GrupalGuiada.Guia;

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
