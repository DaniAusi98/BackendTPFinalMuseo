using Application.MuseumResources.DataTransferObjects;

namespace Application.Eventos.DataTransferObjets
{
    public class RecursoAsignadoPropuestaDto
    {
        public string PropuestaId { get; private set; }
        public PropuestaEventoDto Propuesta { get; private set; }

        public string RecursoId { get; private set; }
        public RecursoDto Recurso { get; private set; }

        public int CantidadAsignada { get; private set; }

        public DateTime Inicio { get; private set; }
        public DateTime Fin { get; private set; }

    }
}
