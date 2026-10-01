using Core.Application;

namespace Application.Eventos.UseCases.Queries.SalasDisponibles
{
    public class SalaDispEventosQuery : IRequestQuery<QueryResult<SalaDisponibleParaEventoDto>>
    {
    }

    public class SalaDisponibleParaEventoDto
    {
        public string Id { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string CodigoSala { get; set; } = string.Empty;
        public int Capacidad { get; set; }
        public string Ubicacion { get; set; } = string.Empty;
        public string TipoSala { get; set; } = string.Empty;
    }
}
