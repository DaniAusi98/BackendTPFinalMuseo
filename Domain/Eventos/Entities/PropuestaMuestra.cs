using Core.Domain.Entities;

namespace Domain.Eventos.Entities
{
    public class PropuestaMuestra : DomainEntity<string>
    {
        public string MuestraTemporalId { get; private set; } = string.Empty;
        public MuestraTemporal MuestraTemporal { get; private set; } = null!;

        public bool FormularioCompletado { get; private set; }
        public bool FormularioEnviado { get; private set; }
        public DateTime? FechaEnvioFormulario { get; private set; }

        protected PropuestaMuestra()
        {
        }

        public PropuestaMuestra(MuestraTemporal muestraTemporal)
        {
            MuestraTemporal = muestraTemporal
                ?? throw new ArgumentNullException(nameof(muestraTemporal));

            MuestraTemporalId = muestraTemporal.Id;
            Id = Guid.NewGuid().ToString();
        }

        public void MarcarFormularioCompletado()
        {
            FormularioCompletado = true;
        }

        public void MarcarFormularioPendiente()
        {
            FormularioCompletado = false;
        }

        public void MarcarFormularioEnviado()
        {
            if (!FormularioCompletado)
            {
                throw new InvalidOperationException(
                    "No se puede enviar un formulario que no está completado.");
            }

            FormularioEnviado = true;
            FechaEnvioFormulario = DateTime.UtcNow;
        }

        public void MarcarFormularioNoEnviado()
        {
            FormularioEnviado = false;
            FechaEnvioFormulario = null;
        }
    }
}