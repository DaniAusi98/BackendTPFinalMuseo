using Core.Domain.Entities;
using Domain.Common.Exceptions;

namespace Domain.PersonalMuseo.Entities.UsuarioInterno
{
    public class AsignacionPersonal : DomainEntity<string>
    {
        public string PersonalId { get; private set; }
        public Personal Personal { get; private set; }

        public string AreaPuestoId { get; private set; }
        public AreaPuesto AreaPuesto { get; private set; }

        public DateTime FechaAsignacion { get; private set; }
        public bool Activa { get; private set; }

        protected AsignacionPersonal()
        {
        }

        public AsignacionPersonal(
            string personalId,
            string areaPuestoId)
        {
            if (string.IsNullOrWhiteSpace(personalId))
                throw new DomainException("PersonalId es obligatorio.");

            if (string.IsNullOrWhiteSpace(areaPuestoId))
                throw new DomainException("AreaPuestoId es obligatorio.");

            Id = Guid.NewGuid().ToString();
            PersonalId = personalId;
            AreaPuestoId = areaPuestoId;
            FechaAsignacion = DateTime.UtcNow;
            Activa = true;
        }
    }
}