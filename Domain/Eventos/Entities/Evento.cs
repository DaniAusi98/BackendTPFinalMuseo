using Domain.ActividadMuseo.Entities;
using Domain.Common.Exceptions;
using Domain.Common.ValueObjets;
using Domain.RecursoMuseo.Entities;
using static Domain.ActividadMuseo.Enums.Enums;
using static Domain.Eventos.Enums.Enums;

namespace Domain.Eventos.Entities
{
    public class Evento : ActividadMuseo.Entities.ActividadMuseo
    {
        public string NombreyApellidoSolicitante { get; private set; }
        public Telefono TelefonoSolicitante { get; private set; }
        public Email EmailSolicitante { get; private set; }
        public string Institucion { get; private set; }
        public TipoEvento TipoEvento { get; private set; }
        public string TituloEvento { get; private set; }
        public string DescripcionEvento { get; private set; } = string.Empty;
        public string FundamentacionEvento { get; private set; } = string.Empty;
        public List<TipoPublico> TipoPublico { get; private set; }
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
            string institucion,
            TipoEvento tipoEvento,
            string tituloEvento,
            string descripcionEvento,
            string fundamentacionEvento,
            List<TipoPublico> tipoPublico,
            int concurrenciaEstimada,
            TimeSlot horario,
            IEnumerable<Sala> salas,
            bool requiereDifusion,
            bool solicitaFlyer,
            IEnumerable<RecursoAsignado>? recursos = null,
            IEnumerable<string?>? urlImagenes = null,
            string? recurrenceRule = null
            )
             : base(
            CategoriaActividad.EventoActividadExterna,
            TipoActividad.Evento,
            concurrenciaEstimada,
            horario,
            salas,
            recursos,
            recurrenceRule

            )
        {
            if (string.IsNullOrWhiteSpace(nombreyApellidoSolicitante))
                throw new ArgumentException("El nombre y apellido del solicitante no puede estar vacío.", nameof(nombreyApellidoSolicitante));
            if (string.IsNullOrWhiteSpace(institucion))
                throw new ArgumentException("La institución no puede estar vacía.", nameof(institucion));
            if (string.IsNullOrWhiteSpace(tituloEvento))
                throw new ArgumentException("El título del evento no puede estar vacío.", nameof(tituloEvento));
            if (tipoPublico is null || tipoPublico.Count == 0)
                throw new ArgumentException("Debe indicar al menos un tipo de público.", nameof(tipoPublico));
            if (tipoPublico.Distinct().Count() != tipoPublico.Count)
                throw new ArgumentException("No debe repetir tipos de público.", nameof(tipoPublico));

            NombreyApellidoSolicitante = nombreyApellidoSolicitante;
            TelefonoSolicitante = telefonoSolicitante;
            EmailSolicitante = emailSolicitante;
            Institucion = institucion;
            TipoEvento = tipoEvento;
            TituloEvento = tituloEvento;
            DescripcionEvento = descripcionEvento?.Trim() ?? string.Empty;
            FundamentacionEvento = fundamentacionEvento?.Trim() ?? string.Empty;
            TipoPublico = [.. tipoPublico];
            RequiereDifusion = requiereDifusion;
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
            string institucion,
            TipoEvento tipoEvento,
            string tituloEvento,
            string descripcionEvento,
            string fundamentacionEvento,
            List<TipoPublico> tipoPublico,
            int ConcurrenciaEstimada,
            bool requiereDifusion,
            bool solicitaFlyer,
            IEnumerable<string?>? urlImagenes = null)
        {
            if (string.IsNullOrWhiteSpace(nombreyApellidoSolicitante))
                throw new ArgumentException("El nombre y apellido del solicitante no puede estar vacío.", nameof(nombreyApellidoSolicitante));
            if (string.IsNullOrWhiteSpace(institucion))
                throw new ArgumentException("La institución no puede estar vacía.", nameof(institucion));
            if (string.IsNullOrWhiteSpace(tituloEvento))
                throw new ArgumentException("El título del evento no puede estar vacío.", nameof(tituloEvento));
            if (tipoPublico is null || tipoPublico.Count == 0)
                throw new ArgumentException("Debe indicar al menos un tipo de público.", nameof(tipoPublico));
            if (tipoPublico.Distinct().Count() != tipoPublico.Count)
                throw new ArgumentException("No debe repetir tipos de público.", nameof(tipoPublico));
            NombreyApellidoSolicitante = nombreyApellidoSolicitante;
            TelefonoSolicitante = telefonoSolicitante;
            EmailSolicitante = emailSolicitante;
            Institucion = institucion;
            TipoEvento = tipoEvento;
            TituloEvento = tituloEvento;
            DescripcionEvento = descripcionEvento?.Trim() ?? string.Empty;
            FundamentacionEvento = fundamentacionEvento?.Trim() ?? string.Empty;
            TipoPublico = tipoPublico.ToList();
            CantidadEstimada = ConcurrenciaEstimada; // puede ser null
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