using Domain.ActividadMuseo.Entities;
using Domain.Common.Exceptions;
using Domain.Common.ValueObjets;
using Domain.RecursoMuseo.Entities;
using static Domain.ActividadMuseo.Enums.Enums;
using static Domain.Eventos.Enums.Enums;

namespace Domain.Eventos.Entities
{
    public class Evento : ActividadBase
    {
        public string NombreyApellidoResponsable { get; private set; }
        public Telefono TelefonoResponsable { get; private set; }
        public Email EmailResponsable { get; private set; }
        public string? Institucion { get; private set; }
        public TipoEvento TipoEvento { get; private set; }
        public string DescripcionEvento { get; private set; } = string.Empty;
        public string FundamentacionEvento { get; private set; } = string.Empty;
        public string? TipoPublico { get; private set; }
        public int? CantidadEstimada { get; private set; }
        public bool RequiereDifusion { get; private set; }
        public List<string> UrlImagenes { get; private set; } = [];
        public bool SolicitaFlyer { get; private set; }



        protected Evento()
        {
        }


        public Evento(
            string nombreyApellidoSolicitante,
            Telefono telefonoSolicitante,
            Email emailSolicitante,
            TipoEvento tipoEvento,
            string tituloEvento,
            TimeSlot horario,
            IEnumerable<Sala> salas,
            bool requiereDifusion,
            bool solicitaFlyer,
            string? descripcionEvento = null,
            string? fundamentacionEvento = null,
            string? tipoPublico = null,
            string? institucion = null,
            int? concurrenciaEstimada = null,
            IEnumerable<RecursoAsignado>? recursos = null,
            IEnumerable<string?>? urlImagenes = null,
            string? recurrenceRule = null
            )
             : base(
            tituloEvento,
            TipoActividad.EventoActividadExterna,
            horario,
            concurrenciaEstimada,
            salas,
            recursos,
            recurrenceRule)
        {
            if (string.IsNullOrWhiteSpace(nombreyApellidoSolicitante))
                throw new ArgumentException("El nombre y apellido del solicitante no puede estar vacío.", nameof(nombreyApellidoSolicitante));


            if (institucion is not null && string.IsNullOrWhiteSpace(institucion))
            {
                throw new ArgumentException(
                    "La institución no puede estar vacía.",
                    nameof(institucion));
            }

            if (tipoPublico is not null && string.IsNullOrWhiteSpace(tipoPublico))
            {
                throw new ArgumentException(
                    "El tipo de publico no puede estar vacía.",
                    nameof(institucion));
            }


            if (string.IsNullOrWhiteSpace(tituloEvento))
                throw new ArgumentException("El título del evento no puede estar vacío.", nameof(tituloEvento));


            NombreyApellidoResponsable = nombreyApellidoSolicitante;
            TelefonoResponsable = telefonoSolicitante;
            EmailResponsable = emailSolicitante;
            Institucion = institucion;
            TipoEvento = tipoEvento;
            DescripcionEvento = descripcionEvento?.Trim() ?? string.Empty;
            FundamentacionEvento = fundamentacionEvento?.Trim() ?? string.Empty;
            TipoPublico = tipoPublico;
            RequiereDifusion = requiereDifusion;
            SolicitaFlyer = solicitaFlyer;
            UrlImagenes = urlImagenes?
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x!)
                .ToList()
                ?? [];
            SolicitaFlyer = solicitaFlyer;
        }

        public void Actualizar(
            string nombreyApellidoSolicitante,
            Telefono telefonoSolicitante,
            Email emailSolicitante,
            TipoEvento tipoEvento,
            string tituloEvento,
            IEnumerable<Sala> salas,
            bool requiereDifusion,
            bool solicitaFlyer,
            string? institucion = null,
            string? descripcionEvento = null,
            string? fundamentacionEvento = null,
             string? tipoPublico = null,
             int? ConcurrenciaEstimada = null,
            IEnumerable<RecursoAsignado>? recursos = null,
            IEnumerable<string?>? urlImagenes = null,
            string? recurrenceRule = null)
        {



            ActualizarRecursos(recursos);
            ActualizarSalas(salas);
            ActualizarRRule(recurrenceRule);
            CambiarCantidadAsistentes(ConcurrenciaEstimada);

            if (string.IsNullOrWhiteSpace(nombreyApellidoSolicitante))
                throw new ArgumentException("El nombre y apellido del solicitante no puede estar vacío.", nameof(nombreyApellidoSolicitante));

            if (string.IsNullOrWhiteSpace(tituloEvento))
                throw new ArgumentException("El título del evento no puede estar vacío.", nameof(tituloEvento));

            if (institucion is not null && string.IsNullOrWhiteSpace(institucion))
            {
                throw new ArgumentException(
                    "La institución no puede estar vacía.",
                    nameof(institucion));
            }

            if (tipoPublico is not null && string.IsNullOrWhiteSpace(tipoPublico))
            {
                throw new ArgumentException(
                    "El tipo de publico no puede estar vacía.",
                    nameof(institucion));
            }
            NombreyApellidoResponsable = nombreyApellidoSolicitante;
            TelefonoResponsable = telefonoSolicitante;
            EmailResponsable = emailSolicitante;
            Institucion = institucion;
            TipoEvento = tipoEvento;
            DescripcionEvento = descripcionEvento?.Trim() ?? string.Empty;
            FundamentacionEvento = fundamentacionEvento?.Trim() ?? string.Empty;
            TipoPublico = tipoPublico;
            RequiereDifusion = requiereDifusion;
            SolicitaFlyer = solicitaFlyer;
            UrlImagenes = urlImagenes?
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x!)
                .ToList()
                ?? [];

        }

        public void ActualizarCantidadEstimada(int cantidadEstimada)
        {
            CantidadEstimada = cantidadEstimada;

        }
        public void ActualizarUrlImagenes(IEnumerable<string?>? urlImagenes)
        {
            UrlImagenes = urlImagenes?
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x!)
                .ToList()
                ?? [];
        }

        public void CancelarEvento()
        {
            if (Estado == EstadoActividad.Cancelada)
                throw new DomainException(
                    "La visita ya está cancelada.");

            CancelarActividad();
        }
        public void MarcarActividadcomoReprogramada()
        {
            if (Estado == EstadoActividad.Cancelada)
                throw new DomainException(
                    "La actividad ya está cancelada.");
            MarcarComoActividadReprogramada();
        }
    }
}