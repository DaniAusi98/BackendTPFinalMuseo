namespace Application.PersonalMuseo.DataTrasnferObjets
{
    public class AsignacionPersonalDto
    {
        public string AsignacionPersonalId { get; private set; }
        public AreaPuestoDto AreaPuesto { get; private set; }
        public string AreaPuestoId { get; private set; }
        public DateTime FechaAsignacion { get; private set; }
        public bool Activa { get; private set; }
    }
}
