namespace Application.PersonalMuseo.DataTrasnferObjets
{
    public class AreaPuestoDto
    {
        public string AreaPuestoId { get; private set; }
        public string AreaId { get; private set; }
        public AreaDto Area { get; private set; }

        public string PuestoId { get; private set; }
        public PuestoDto Puesto { get; private set; }
    }
}
