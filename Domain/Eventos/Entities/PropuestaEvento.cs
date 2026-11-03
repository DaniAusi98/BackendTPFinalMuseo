using Core.Domain.Entities;

namespace Domain.Eventos.Entities
{
    public class PropuestaEvento : DomainEntity<string>
    {


        public string EventoId { get; private set; }
        public Evento Evento { get; private set; }

        public bool FormularioCompletado { get; private set; }
        public bool FormularioEnviado { get; private set; }
        public DateTime? FechaEnvioFormulario { get; private set; }

        protected PropuestaEvento()
        {
        }

        public PropuestaEvento(Evento evento)
        {
            Evento = evento
                ?? throw new ArgumentNullException(nameof(evento));

            EventoId = evento.Id;
            Id = Guid.NewGuid().ToString();
            FormularioCompletado = false;
            FormularioEnviado = false;

        }

        public void RegistrarEnvioFormulario()
        {
            FormularioEnviado = true;
            FechaEnvioFormulario = DateTime.UtcNow;
        }
        public void RegistrarFormularioCompletado()
        {
            if (FormularioCompletado)
                throw new InvalidOperationException(
                    "El formulario del evento ya fue completado.");

            FormularioCompletado = true;
        }

    }
}
