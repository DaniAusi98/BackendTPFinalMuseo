using Core.Domain.Entities;
using Domain.Common.Exceptions;

namespace Domain.PersonalMuseo.Entities.UsuarioInterno
{
    public class AreaPuesto : DomainEntity<string>
    {
        public string AreaId { get; private set; }
        public Area Area { get; private set; }

        public string PuestoId { get; private set; }
        public Puesto Puesto { get; private set; }

        protected AreaPuesto()
        {
        }

        public AreaPuesto(string areaId, string puestoId)
        {
            Id = Guid.NewGuid().ToString();

            SetAreaId(areaId);
            SetPuestoId(puestoId);
        }

        public void SetAreaId(string areaId)
        {
            if (string.IsNullOrWhiteSpace(areaId))
                throw new DomainException("AreaId no puede ser nulo o vacío.");

            AreaId = areaId.Trim();
        }

        public void SetPuestoId(string puestoId)
        {
            if (string.IsNullOrWhiteSpace(puestoId))
                throw new DomainException("PuestoId no puede ser nulo o vacío.");

            PuestoId = puestoId.Trim();
        }
    }
}