using Domain.Common.Exceptions;
using Domain.Common.ValueObjets;
using Domain.RecursoMuseo.Entities;
using static Domain.ActividadMuseo.Enums.Enums;
using static Domain.VisitasGrupales.Entities.GrupalGuiada.Enums.Enums;

namespace Domain.VisitasGrupales.Entities.GrupalGuiada
{
    public class VisitaGrupalGuiada : ActividadMuseo.Entities.ActividadMuseo
    {

        public string UsuarioVisitanteId { get; private set; }
        public string NombreSolicitante { get; private set; }
        public string ApellidoSolicitante { get; private set; }
        public Email EmailSolicitante { get; private set; }
        public Telefono TelefonoSolicitante { get; private set; }
        public string Institucion { get; private set; }
        public NivelEducativo? NivelEducativo { get; private set; }
        public string? AnioGrado { get; private set; }
        public Email EmailInstitucion { get; private set; }
        public Telefono TelefonoInstitucion { get; private set; }
        public string PaisInstitucion { get; private set; }
        public string ProvinciaInstitucion { get; private set; }
        public string LocalidadInstitucion { get; private set; }
        public string DiversidadFuncionalDescripcion { get; private set; } = string.Empty;
        public string MotivoRelacionVisita { get; private set; } = string.Empty;
        public string Observaciones { get; private set; } = string.Empty;
        public DateTime FechaCreacion { get; private set; } = DateTime.UtcNow;
        public List<TematicaVisita> Tematicas { get; private set; } = [];
        public EstadoConfirmacionVisita EstadoConfirmacion { get; private set; }

        protected VisitaGrupalGuiada()
        {
        }


        public VisitaGrupalGuiada(
            string usuarioVisitanteId,
            string nombreSolicitante,
            string apellidoSolicitante,
            Email emailSolicitante,
            Telefono telefonoSolicitante,
            NivelEducativo? nivelEducativo,
            string? anioGrado,
            int cantidadPersonas,
            string institucion,
            Email emailInstitucion,
            Telefono telefonoInstitucion,
            string paisInstitucion,
            string provinciaInstitucion,
            string ciudadInstitucion,
            string descripcionDiversidad,
            string motivoVisita,
            string observaciones,
            TimeSlot horario,
            IEnumerable<TematicaVisita> tematicas,
            IEnumerable<Sala> salas
        )
        : base(
            "Visita Guiada",
            TipoActividad.VisitaGrupalGuiada,
            cantidadPersonas,
            horario,
            salas)
        {
            ValidarNivelEducativo(
                nivelEducativo,
                anioGrado);


            UsuarioVisitanteId = usuarioVisitanteId;
            NombreSolicitante = nombreSolicitante?.Trim() ?? string.Empty;
            ApellidoSolicitante = apellidoSolicitante?.Trim() ?? string.Empty;
            EmailSolicitante = emailSolicitante;
            TelefonoSolicitante = telefonoSolicitante;

            NivelEducativo = nivelEducativo;

            AnioGrado = anioGrado;

            Institucion = institucion;

            EmailInstitucion = emailInstitucion;

            TelefonoInstitucion = telefonoInstitucion;

            PaisInstitucion = paisInstitucion;

            ProvinciaInstitucion = provinciaInstitucion;

            LocalidadInstitucion = ciudadInstitucion;


            DiversidadFuncionalDescripcion =
                descripcionDiversidad?.Trim() ?? string.Empty;


            MotivoRelacionVisita =
                motivoVisita?.Trim() ?? string.Empty;


            Observaciones =
                observaciones?.Trim() ?? string.Empty;


            AsignarTematicas(tematicas);


            EstadoConfirmacion =
                EstadoConfirmacionVisita.PendienteConfirmar;
        }


        public void ActualizarDatos(
            string institucion,
            Email emailInstitucion,
            Telefono telefonoInstitucion,
            string paisInstitucion,
            string provinciaInstitucion,
            string localidadInstitucion,
            NivelEducativo? nivelEducativo,
            string? anioGrado,
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

            ValidarNivelEducativo(nivelEducativo, anioGrado);

            Institucion = institucion.Trim();
            EmailInstitucion = emailInstitucion;
            TelefonoInstitucion = telefonoInstitucion;

            PaisInstitucion = paisInstitucion.Trim();
            ProvinciaInstitucion = provinciaInstitucion.Trim();
            LocalidadInstitucion = localidadInstitucion.Trim();

            NivelEducativo = nivelEducativo;
            AnioGrado = anioGrado?.Trim();

            DiversidadFuncionalDescripcion =
                descripcionDiversidad?.Trim() ?? string.Empty;

            MotivoRelacionVisita =
                motivoVisita?.Trim() ?? string.Empty;

            Observaciones =
                observaciones?.Trim() ?? string.Empty;
        }
        private static void ValidarNivelEducativo(
            NivelEducativo? nivel,
            string? anioGrado)
        {
            if (nivel != null && anioGrado == null)
                throw new DomainException(
                    "Debe especificar el año o grado.");

            if (nivel == null && anioGrado != null)
                throw new DomainException(
                    "No puede especificar año/grado sin nivel educativo.");
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
                    "La visita no está pendiente de confirmación.");

            EstadoConfirmacion =
                EstadoConfirmacionVisita.Confirmada;
        }


        public void CancelarVisitaGuiada()
        {
            if (Estado == EstadoActividad.Cancelada)
                throw new DomainException(
                    "La visita ya está cancelada.");

            CancelarActividad();
        }


        public void CambiarEstadoAReprogramada()
        {
            if (Estado == EstadoActividad.Cancelada)
                throw new DomainException(
                    "No se puede reprogramar una visita cancelada.");

            MarcarComoActividadReprogramada();
        }
    }
}
