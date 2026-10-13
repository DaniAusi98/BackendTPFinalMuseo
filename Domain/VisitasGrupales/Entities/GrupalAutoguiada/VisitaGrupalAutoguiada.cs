using Domain.Common.Exceptions;
using Domain.Common.ValueObjets;
using Domain.RecursoMuseo.Entities;
using Domain.VisitasGrupales.Entities.GrupalGuiada;
using static Domain.ActividadMuseo.Enums.Enums;
using static Domain.VisitasGrupales.Entities.GrupalGuiada.Enums.Enums;

namespace Domain.VisitasGrupales.Entities.GrupalAutoguiada
{
    public class VisitaGrupalAutoguiada : ActividadMuseo.Entities.ActividadMuseo
    {
        public string UsuarioVisitanteId { get; private set; }
        public string NombreSolicitante { get; private set; }
        public string ApellidoSolicitante { get; private set; }
        public Email EmailSolicitante { get; private set; }
        public Telefono TelefonoSolicitante { get; private set; }
        public string Institucion { get; private set; }
        public Email EmailInstitucion { get; private set; }
        public string PaisInstitucion { get; private set; }
        public string ProvinciaInstitucion { get; private set; }
        public string LocalidadInstitucion { get; private set; }
        public string DiversidadFuncional { get; private set; } = string.Empty;
        public string Observaciones { get; private set; } = string.Empty;
        public EstadoConfirmacionVisita EstadoConfirmacion { get; private set; }
        public List<TematicaVisita> Tematicas { get; private set; } = [];



        protected VisitaGrupalAutoguiada()
        {
        }


        public VisitaGrupalAutoguiada(
            string usuarioVisitanteId,
            string nombreSolicitante,
            string apellidoSolicitante,
            Email emailSolicitante,
            Telefono telefonoSolicitante,
            int cantidadPersonas,
            string institucion,
            Email emailInstitucion,
            string paisInstitucion,
            string provinciaInstitucion,
            string ciudadInstitucion,
            string descripcionDiversidad,
            string observaciones,
            TimeSlot horario,
            IEnumerable<TematicaVisita> tematicas,
            IEnumerable<Sala> salas
        )
        : base(
            "Visita Autoguiada",
            TipoActividad.VisitaGrupalAutoguiada,
            cantidadPersonas,
            horario,
            salas)
        {

            if (usuarioVisitanteId == null)
                throw new ArgumentNullException(
                    nameof(usuarioVisitanteId));


            if (string.IsNullOrWhiteSpace(institucion))
                throw new DomainException(
                    "La institución es obligatoria.");


            ArgumentNullException.ThrowIfNull(emailInstitucion);
            if (string.IsNullOrWhiteSpace(paisInstitucion))
                throw new DomainException(
                    "El país de la institución es obligatorio.");

            if (string.IsNullOrWhiteSpace(provinciaInstitucion))
                throw new DomainException(
                    "La provincia de la institución es obligatoria.");





            if (string.IsNullOrWhiteSpace(ciudadInstitucion))
                throw new DomainException(
                    "La ciudad de la institución es obligatoria.");


            UsuarioVisitanteId = usuarioVisitanteId;

            NombreSolicitante = nombreSolicitante?.Trim() ?? string.Empty;
            ApellidoSolicitante = apellidoSolicitante?.Trim() ?? string.Empty;
            EmailSolicitante = emailSolicitante;
            TelefonoSolicitante = telefonoSolicitante;

            Institucion = institucion.Trim();

            EmailInstitucion = emailInstitucion;
            PaisInstitucion = paisInstitucion.Trim();

            ProvinciaInstitucion = provinciaInstitucion.Trim();


            LocalidadInstitucion = ciudadInstitucion.Trim();


            DiversidadFuncional =
                string.IsNullOrWhiteSpace(descripcionDiversidad)
                ? string.Empty
                : descripcionDiversidad.Trim();


            Observaciones =
                string.IsNullOrWhiteSpace(observaciones)
                ? string.Empty
                : observaciones.Trim();
            AsignarTematicas(tematicas);


            EstadoConfirmacion = EstadoConfirmacionVisita.PendienteConfirmar;
        }


        public void ActualizarDatos(
          string institucion,
          Email emailInstitucion,
          Telefono telefonoInstitucion,
          string paisInstitucion,
          string provinciaInstitucion,
          string localidadInstitucion,

          string? descripcionDiversidad,
          string? motivoVisita,
          string? observaciones)
        {
            if (string.IsNullOrWhiteSpace(institucion))
                throw new DomainException(
                    "La institución es obligatoria.");

            if (string.IsNullOrWhiteSpace(paisInstitucion))
                throw new DomainException(
                    "El país de la institución es obligatorio.");

            if (string.IsNullOrWhiteSpace(provinciaInstitucion))
                throw new DomainException(
                    "La provincia de la institución es obligatoria.");

            if (string.IsNullOrWhiteSpace(localidadInstitucion))
                throw new DomainException(
                    "La localidad de la institución es obligatoria.");

            ArgumentNullException.ThrowIfNull(emailInstitucion);
            ArgumentNullException.ThrowIfNull(telefonoInstitucion);


            Institucion = institucion.Trim();
            EmailInstitucion = emailInstitucion;

            PaisInstitucion = paisInstitucion.Trim();
            ProvinciaInstitucion = provinciaInstitucion.Trim();
            LocalidadInstitucion = localidadInstitucion.Trim();



            DiversidadFuncional =
                descripcionDiversidad?.Trim() ?? string.Empty;



            Observaciones =
                observaciones?.Trim() ?? string.Empty;
        }
        public void AsignarTematicas(
           IEnumerable<TematicaVisita> tematicas)
        {
            ArgumentNullException.ThrowIfNull(tematicas);

            var lista = tematicas.ToList();

            if (lista.Select(x => x.Id)
                .Distinct()
                .Count() != lista.Count)
            {
                throw new DomainException(
                    "No se pueden repetir temáticas.");
            }

            Tematicas = lista;
        }



        public void Confirmar()
        {
            if (Estado == EstadoActividad.Cancelada)
                throw new DomainException(
                    "No se puede confirmar una visita cancelada.");

            if (EstadoConfirmacion != EstadoConfirmacionVisita.PendienteConfirmar)
                throw new DomainException(
                    "Solo se pueden confirmar visitas pendientes.");

            EstadoConfirmacion = EstadoConfirmacionVisita.Confirmada;
        }

        public void CambiarEstadoAReprogramada()
        {
            if (Estado == EstadoActividad.Cancelada)
                throw new DomainException(
                    "No se puede reprogramar una visita cancelada.");

            MarcarComoActividadReprogramada();
        }

        public void Rechazar()
        {
            if (Estado == EstadoActividad.Cancelada)
                throw new DomainException(
                    "La visita ya está cancelada.");

            CancelarActividad();
        }
    }
}
