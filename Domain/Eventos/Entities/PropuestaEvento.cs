using Core.Domain.Entities;
using Domain.Common.Exceptions;
using Domain.Common.ValueObjets;
using Domain.RecursoMuseo.Entities;
using static Domain.Eventos.Enums.Enums;

namespace Domain.Eventos.Entities
{
    public class PropuestaEvento : DomainEntity<string>
    {
        public string NombreApellidoResponsable { get; private set; }
        public Telefono TelefonoResponsable { get; private set; }
        public Email EmailResponsable { get; private set; }
        public string TituloPropuesta { get; private set; }
        public TipoEventoActividad TipoEvento { get; private set; }
        public DateTime Fecha { get; private set; }
        public DateTime FechaHasta { get; private set; }
        public bool FormularioEventoCompletado { get; private set; }
        public List<Sala> Salas { get; private set; } = [];
        public List<PropuestasRecursos> Recursos { get; private set; } = [];
        public string? RRule { get; private set; }
        public bool FormularioEnviado { get; private set; } = false;
        public DateTime? FechaEnvioFormulario { get; private set; }

        protected PropuestaEvento()
        {
        }

        public PropuestaEvento(
            string nombreApellidoResponsable,
            Telefono telefonoResponsable,
            Email emailResponsable,
            string titulo,
            TipoEventoActividad tipoEvento,
            DateTime fecha,
            DateTime fechaHasta,
            IEnumerable<Sala> salas,
            IEnumerable<PropuestasRecursos> recursos = null,
            string? rrule = null)
        {
            if (string.IsNullOrEmpty(nombreApellidoResponsable))
                throw new ArgumentException(
                    "El nombre y apellido del responsable de la muestra no puede estar vacío.",
                    nameof(nombreApellidoResponsable));

            if (string.IsNullOrEmpty(titulo))
                throw new ArgumentException(
                    "El titulo de la propuesta no puede estar vacío.",
                    nameof(titulo));

            if (fechaHasta <= fecha)
                throw new DomainException(
                    "La fecha de fin debe ser posterior a la fecha de inicio.");

            if (salas is null)
                throw new DomainException(
                    "La actividad debe tener al menos una sala.");

            var salasLista = salas.ToList();

            if (salasLista.Count == 0)
                throw new DomainException(
                    "La actividad debe tener al menos una sala.");

            if (salasLista.Any(s => s is null))
                throw new DomainException("No puede haber salas nulas.");


            if (recursos is not null)
            {
                if (recursos.Any(r => r is null))
                    throw new DomainException("No puede haber recursos nulos.");
                if (recursos.GroupBy(r => r.RecursoId).Any(g => g.Count() > 1))
                    throw new DomainException("No puede haber recursos repetidos.");
            }

            Id = Guid.NewGuid().ToString();
            NombreApellidoResponsable = nombreApellidoResponsable;
            TelefonoResponsable = telefonoResponsable
                ?? throw new ArgumentNullException(nameof(telefonoResponsable));
            EmailResponsable = emailResponsable
                ?? throw new ArgumentNullException(nameof(emailResponsable));
            TituloPropuesta = titulo;
            TipoEvento = tipoEvento;
            Fecha = fecha;
            FechaHasta = fechaHasta;
            FormularioEventoCompletado = false;
            Salas = salasLista;
            RRule = rrule;
        }

        public void ActualizarPropuesta(
            string nombreApellidoResponsable,
            Telefono telefonoResponsable,
            Email emailResponsable,
            string titulo,
            TipoEventoActividad tipoEvento,
            DateTime fecha,
            DateTime fechaHasta,
            IEnumerable<Sala> salas,
            IEnumerable<PropuestasRecursos> recursos = null,
            string? rrule = null)
        {
            if (string.IsNullOrEmpty(nombreApellidoResponsable))
                throw new ArgumentException(
                    "El nombre y apellido del responsable de la muestra no puede estar vacío.",
                    nameof(nombreApellidoResponsable));

            if (string.IsNullOrEmpty(titulo))
                throw new ArgumentException(
                    "El titulo de la propuesta no puede estar vacío.",
                    nameof(titulo));

            if (fechaHasta <= fecha)
                throw new DomainException(
                    "La fecha de fin debe ser posterior a la fecha de inicio.");

            if (salas is null)
                throw new DomainException(
                    "La actividad debe tener al menos una sala.");

            var salasLista = salas.ToList();

            if (salasLista.Count == 0)
                throw new DomainException(
                    "La actividad debe tener al menos una sala.");

            if (salasLista.Any(s => s is null))
                throw new DomainException("No puede haber salas nulas.");

            if (recursos is not null)
            {
                if (recursos.Any(r => r is null))
                    throw new DomainException("No puede haber recursos nulos.");
                if (recursos.GroupBy(r => r.RecursoId).Any(g => g.Count() > 1))
                    throw new DomainException("No puede haber recursos repetidos.");
            }



            NombreApellidoResponsable = nombreApellidoResponsable;
            TelefonoResponsable = telefonoResponsable
                ?? throw new ArgumentNullException(nameof(telefonoResponsable));
            EmailResponsable = emailResponsable
                ?? throw new ArgumentNullException(nameof(emailResponsable));
            TituloPropuesta = titulo;
            TipoEvento = tipoEvento;
            Fecha = fecha;
            FechaHasta = fechaHasta;
            Salas = salasLista;
            RRule = rrule;
        }
        public void MarcarFormularioEnviado()
        {
            FormularioEnviado = true;
            FechaEnvioFormulario = DateTime.UtcNow;
        }

        public void MarcarFormularioPendiente()
        {
            FormularioEnviado = false;
            FechaEnvioFormulario = null;
        }
    }
}
