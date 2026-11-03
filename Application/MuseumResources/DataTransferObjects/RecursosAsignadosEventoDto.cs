namespace Application.MuseumResources.DataTransferObjects
{
    public class RecursosAsignadosEventoDto
    {
        public string RecursoId { get; private set; }
        public RecursoDto Recurso { get; private set; }

        public int CantidadAsignada { get; private set; }

        public DateTime Inicio { get; private set; }
        public DateTime Fin { get; private set; }
    }
}
