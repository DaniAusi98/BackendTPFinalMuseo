namespace Application.MuseumResources.DataTransferObjects
{
    public class SalaDto
    {
        public string Id { get; private set; }
        public string Nombre { get; private set; } = string.Empty;
        public string Estado { get; private set; }
        public string CodigoSala { get; private set; } = string.Empty;
        public string TipoSala { get; private set; }
        public int Capacidad { get; private set; }
        public string Ubicacion { get; private set; }

    }
}
