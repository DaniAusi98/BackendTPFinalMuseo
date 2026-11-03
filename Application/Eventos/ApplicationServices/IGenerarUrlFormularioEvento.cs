using static Domain.Eventos.Enums.Enums;

namespace Application.Eventos.ApplicationServices
{
    public interface IGenerarUrlFormularioEvento
    {
        public string GenerarUrlFormularioEvento(string EventoId, TipoEvento tipoEventoActividad);
    }
}
