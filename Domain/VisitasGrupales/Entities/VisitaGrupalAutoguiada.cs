using Domain.Common.Exceptions;
using Domain.Common.ValueObjets;
using Domain.RecursoMuseo.Entities;
using static Domain.ActividadMuseo.Enums.Enums;
using static Domain.VisitasGrupales.Enums.Enums;

namespace Domain.VisitasGrupales.Entities
{
    public class VisitaGrupalAutoguiada : ActividadMuseo.Entities.ActividadMuseo
    {
        public string UsuarioVisitanteId { get; private set; }
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
            CategoriaActividad.VisitaGrupal,
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


        public void ActualizarProvincia(string nuevaProvincia)
        {
            if (string.IsNullOrWhiteSpace(nuevaProvincia))
                throw new DomainException(
                    "La provincia de la institución es obligatoria.");

            ProvinciaInstitucion = nuevaProvincia.Trim();
        }


        public void ActualizarPais(string nuevoPais)
        {
            if (string.IsNullOrWhiteSpace(nuevoPais))
                throw new DomainException(
                    "El país de la institución es obligatorio.");
            PaisInstitucion = nuevoPais.Trim();
        }


        public void ActualizarLocalidad(string nuevaCiudad)
        {
            if (string.IsNullOrWhiteSpace(nuevaCiudad))
                throw new DomainException(
                    "La ciudad de la institución es obligatoria.");

            LocalidadInstitucion = nuevaCiudad.Trim();
        }


        public void ActualizarDiversidadFuncional(string diversidad)
        {
            DiversidadFuncional =
                string.IsNullOrWhiteSpace(diversidad)
                ? string.Empty
                : diversidad.Trim();
        }


        public void ActualizarObservaciones(string nuevasObservaciones)
        {
            Observaciones =
                string.IsNullOrWhiteSpace(nuevasObservaciones)
                ? string.Empty
                : nuevasObservaciones.Trim();
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
