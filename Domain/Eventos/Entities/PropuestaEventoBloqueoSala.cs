using Core.Domain.Entities;

namespace Domain.Eventos.Entities
{
    public class PropuestaEventoBloqueoSala : DomainEntity<string>
    {
        public string PropuestaEventoId { get; set; }
        public string BloqueoSalaId { get; set; }

        protected PropuestaEventoBloqueoSala()
        {

        }
        public PropuestaEventoBloqueoSala(
            string propuestaEventoId,
            string bloqueoSalaId)
        {
            if (string.IsNullOrWhiteSpace(propuestaEventoId))
                throw new ArgumentException(
                    "El id de la propuesta no puede estar vacío.",
                    nameof(propuestaEventoId));

            if (string.IsNullOrWhiteSpace(bloqueoSalaId))
                throw new ArgumentException(
                    "El id del bloqueo de sala no puede estar vacío.",
                    nameof(bloqueoSalaId));

            Id = Guid.NewGuid().ToString();
            PropuestaEventoId = propuestaEventoId;
            BloqueoSalaId = bloqueoSalaId;
        }
    }
}