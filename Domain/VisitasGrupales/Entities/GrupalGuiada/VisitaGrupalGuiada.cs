using Domain.Common.Exceptions;
using Domain.Common.ValueObjets;
using Domain.RecursoMuseo.Entities;
using static Domain.ActividadMuseo.Enums.Enums;
using static Domain.VisitasGrupales.Enums.Enums;

namespace Domain.VisitasGrupales.Entities.GrupalGuiada
{
    public class VisitaGrupalGuiada : ActividadMuseo.Entities.ActividadMuseo
    {
        public string UsuarioVisitanteId { get; private set; }

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
            CategoriaActividad.VisitaGrupal,
            TipoActividad.VisitaGrupalGuiada,
            cantidadPersonas,
            horario,
            salas)
        {
            ValidarNivelEducativo(
                nivelEducativo,
                anioGrado);


            UsuarioVisitanteId = usuarioVisitanteId;

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


        public void ActualizarInstitucion(string nuevaInstitucion)
        {
            if (string.IsNullOrWhiteSpace(nuevaInstitucion))
                throw new DomainException(
                    "La institución es obligatoria.");

            Institucion = nuevaInstitucion.Trim();
        }


        public void ActualizarEmailInstitucion(Email nuevoEmail)
        {
            ArgumentNullException.ThrowIfNull(nuevoEmail);

            EmailInstitucion = nuevoEmail;
        }
        public void ActualizarPais(string nuevoPais)
        {
            if (string.IsNullOrWhiteSpace(nuevoPais))
                throw new DomainException(
                    "El pais de la institución es obligatoria.");

            PaisInstitucion  = nuevoPais.Trim();
        }



        public void ActualizarProvincia(string nuevaProvincia)
        {
            if (string.IsNullOrWhiteSpace(nuevaProvincia))
                throw new DomainException(
                    "La provincia de la institución es obligatoria.");

            ProvinciaInstitucion = nuevaProvincia.Trim();
        }


       

        public void ActualizarLocalidad(string nuevaLocalidad)
        {
            if (string.IsNullOrWhiteSpace(nuevaLocalidad))
                throw new DomainException(
                    "La localidad de la institución es obligatoria.");

            LocalidadInstitucion = nuevaLocalidad.Trim();
        }


        public void ActualizarDiversidadFuncional(string descripcion)
        {
            DiversidadFuncionalDescripcion =
                string.IsNullOrWhiteSpace(descripcion)
                ? string.Empty
                : descripcion.Trim();
        }


        public void ActualizarObservaciones(string nuevasObservaciones)
        {
            Observaciones =
                string.IsNullOrWhiteSpace(nuevasObservaciones)
                ? string.Empty
                : nuevasObservaciones.Trim();
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
