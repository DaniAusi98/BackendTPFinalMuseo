using Core.Domain.Entities;
using Domain.Common.Exceptions;
namespace Domain.ActividadMuseo.Entities
{
    public class ActividadException:DomainEntity<string>
    {

        public string ActividadMuseoId { get; private set;}
        public DateTime FechaExcluir { get; private set;}
        public string? Motivo { get;private set;}

        private ActividadException() { }

        public ActividadException(
            string actividadMuseoId,
            DateTime fechaExcluir,
            string? motivo)
        {
            if (string.IsNullOrWhiteSpace(actividadMuseoId))
                throw new DomainException(
                    "La excepción debe estar asociada a una actividad.");

            Id = Guid.NewGuid().ToString();

            ActividadMuseoId = actividadMuseoId;
            FechaExcluir = fechaExcluir;   
            Motivo = motivo;
        }
    }
}