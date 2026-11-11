namespace Application.MuseumResources.DataTransferObjects
{
    public class RecursoDto
    {
        public string Id { get; set; }
        public string NombreRecurso { get; private set; } = string.Empty;
        public string TipoRecurso { get; private set; }
        public string Descripcion { get; private set; } = string.Empty;
        public string Estado { get; private set; }
        public int CantidadTotal { get; private set; }
    }
}
