using Core.Domain.Entities;

using System;
using Domain.Common.Exceptions;

namespace Domain.PersonalMuseo.Entities.UsuarioInterno
{
    public class AreaPuesto : DomainEntity<string>
    {
        public string    AreaId { get; private set; }
        public Area Area { get; private set; }

        public string PuestoId { get; private set; }
        public Puesto Puesto { get; private set; }

        public AreaPuesto()
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
            if (areaId == null || areaId.Trim() == "")
                throw new DomainException("AreaId no puede ser nulo o vacío.");
            AreaId = areaId;
        }

        public void SetPuestoId(string puestoId)
        {
            if (puestoId == null || puestoId.Trim() == "")
                throw new DomainException("PuestoId no puede ser nulo o vacío.");
            PuestoId = puestoId;
        }
    }
}
