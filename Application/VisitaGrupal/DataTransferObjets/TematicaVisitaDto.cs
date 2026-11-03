using Application.MuseumResources.DataTransferObjects;

namespace Application.VisitaGrupal.DataTransferObjets
{
    public class TematicaVisitaDto
    {
        public string Id { get; set; }
        public string Nombre { get; private set; } = string.Empty;
        public string Descripcion { get; private set; } = string.Empty;
        public bool Disponible { get; private set; }
        public List<SalaDto> Salas { get; private set; } = [];
    }
}
