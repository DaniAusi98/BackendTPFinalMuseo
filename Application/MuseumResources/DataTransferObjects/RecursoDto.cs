using static Domain.RecursoMuseo.Enums.Enums;

namespace Application.MuseumResources.DataTransferObjects
{
    public class RecursoDto
    {
        public string NombreRecurso { get; private set; } = string.Empty;
        public TipoRecurso TipoRecurso { get; private set; }
        public string Descripcion { get; private set; } = string.Empty;
        public EstadoRecurso Estado { get; private set; }
        public int CantidadTotal { get; private set; }
    }
}
