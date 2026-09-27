using Core.Domain.Entities;

using System;
using Domain.Common.Exceptions;

namespace Domain.PersonalMuseo.Entities.UsuarioInterno
{
    public class PersonalInterno : DomainEntity<string>
    {
        public string IdentityUserId { get; private set; }
        public int AreaId { get; private set; }
        public Area Area { get; private set; }

        public int PuestoId { get; private set; }
        public Puesto Puesto { get; private set; }

        public PersonalInterno()
        {
        }

        public PersonalInterno(string identityUserId, int areaId, int puestoId)
        {
            Id= Guid.NewGuid().ToString();
            SetIdentityUserId(identityUserId);
            SetAreaId(areaId);
            SetPuestoId(puestoId);
        }

        public void SetIdentityUserId(string identityUserId)
        {
            if (string.IsNullOrWhiteSpace(identityUserId))
                throw new DomainException("IdentityUserId no puede estar vacío.");

            IdentityUserId = identityUserId.Trim();
        }

        public void SetAreaId(int areaId)
        {
            if (areaId <= 0) throw new DomainException("AreaId debe ser mayor a 0.");
            AreaId = areaId;
        }

        public void SetPuestoId(int puestoId)
        {
            if (puestoId <= 0) throw new DomainException("PuestoId debe ser mayor a 0.");
            PuestoId = puestoId;
        }
    }
}
