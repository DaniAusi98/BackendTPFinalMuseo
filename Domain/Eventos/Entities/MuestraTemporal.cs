using Domain.ActividadMuseo.Entities;
using Domain.Common.Exceptions;
using Domain.Common.ValueObjets;
using Domain.Eventos.ValueObjets;
using Domain.RecursoMuseo.Entities;
using static Domain.ActividadMuseo.Enums.Enums;

namespace Domain.Eventos.Entities
{
    public class MuestraTemporal : ActividadBase
    {
        public string NombreApellidoResponsable { get; private set; }
        public Telefono TelefonoResponsable { get; private set; }
        public Email EmailResponsable { get; private set; }
        public string? Institucion { get; private set; }
        public Email? EmailInstitucion { get; private set; }
        public string TituloPropuesta { get; private set; }
        public string FundamentacionObjetivoMuestra { get; private set; }
        public string Descripcion { get; private set; }
        public LogisticaMuestraTemporal LogisticaMuestraTemporal { get; private set; }
        public string Publico { get; private set; }
        public bool RequiereDifusion { get; private set; }
        public List<string> UrlImagenesMuestra { get; private set; } = [];
        public bool SolicitaFlyer { get; private set; }

        public MuestraTemporal(
            string nombreResponsable,
            Telefono telefonoResponsable,
            Email emailResponsable,
            string tituloPropuesta,
            string descripcion,
            string fundamentacionMuestra,
            LogisticaMuestraTemporal logisticaMuestraTemporal,
            string publico,
            bool requiereDifusion,
            IEnumerable<Sala> salas,
            TimeSlot fechaMuestra,
            string? institucion = null,
            Email? emailInstitucion = null,
            IEnumerable<string?>? urlImagenes = null,
            IEnumerable<RecursoAsignado>? recursos = null)
            : base(
            tituloPropuesta,
            TipoActividad.MuestraExposicionTemporal,
            fechaMuestra,
            salas: salas,
            recursos: recursos
            )


        {
            if (string.IsNullOrEmpty(nombreResponsable))
                throw new ArgumentNullException("El nombre y apellido del responsable de la muestra no puede estar vacío.",
                nameof(nombreResponsable));
            if (string.IsNullOrEmpty(tituloPropuesta))
                throw new ArgumentNullException("El titulo de la propuesta no puede estar vacío.",
                nameof(tituloPropuesta));
            if (string.IsNullOrEmpty(descripcion))
                throw new ArgumentNullException("La descripcion de la propuesta no puede estar vacío.",
                nameof(descripcion));
            if (string.IsNullOrEmpty(fundamentacionMuestra))
                throw new ArgumentNullException("La fundamentacion de la propuesta no puede estar vacío.",
                nameof(fundamentacionMuestra));
            if (string.IsNullOrEmpty(publico))
                throw new ArgumentNullException("El publico de la propuesta no puede estar vacío.",
                nameof(publico));

            NombreApellidoResponsable = nombreResponsable;
            TelefonoResponsable = telefonoResponsable ?? throw new ArgumentNullException(nameof(telefonoResponsable));
            EmailResponsable = emailResponsable ?? throw new ArgumentNullException(nameof(emailResponsable));
            TituloPropuesta = tituloPropuesta;
            Descripcion = descripcion;
            FundamentacionObjetivoMuestra = fundamentacionMuestra;
            LogisticaMuestraTemporal = logisticaMuestraTemporal ?? throw new ArgumentNullException(nameof(logisticaMuestraTemporal));
            Publico = publico;
            RequiereDifusion = requiereDifusion;
            Institucion = institucion;
            EmailInstitucion = emailInstitucion;
            UrlImagenesMuestra = urlImagenes?
              .Where(x => !string.IsNullOrWhiteSpace(x))
              .Select(x => x!)
              .ToList()
              ?? [];

        }

        private MuestraTemporal()
        {

        }

        public void ActualizarMuestra(
            string nombreResponsable,
            Telefono telefonoResponsable,
            Email emailResponsable,
            string tituloPropuesta,
            string descripcion,
            string fundamentacionMuestra,
            LogisticaMuestraTemporal logisticaMuestraTemporal,
            string publico,
            bool requiereDifusion,
            IEnumerable<Sala> salas,
            string? institucion = null,
            Email? emailInstitucion = null,
            IEnumerable<string?>? urlImagenes = null,
            IEnumerable<RecursoAsignado>? recursos = null

            )
        {
            ActualizarTitulo(tituloPropuesta);
            ActualizarSalas(salas);
            ActualizarRecursos(recursos);

            if (string.IsNullOrEmpty(nombreResponsable))
                throw new ArgumentException("El nombre y apellido del responsable de la muestra no puede estar vacío.",
                nameof(nombreResponsable));
            if (string.IsNullOrEmpty(tituloPropuesta))
                throw new ArgumentException("El titulo de la propuesta no puede estar vacío.",
                nameof(tituloPropuesta));
            if (string.IsNullOrEmpty(descripcion))
                throw new ArgumentException("La descripcion de la propuesta no puede estar vacío.",
                nameof(descripcion));
            if (string.IsNullOrEmpty(fundamentacionMuestra))
                throw new ArgumentException("La fundamentacion de la propuesta no puede estar vacío.",
                nameof(fundamentacionMuestra));
            if (string.IsNullOrEmpty(publico))
                throw new ArgumentException("El publico de la propuesta no puede estar vacío.",
                nameof(publico));


            NombreApellidoResponsable = nombreResponsable;
            TelefonoResponsable = telefonoResponsable ?? throw new ArgumentNullException(nameof(telefonoResponsable));
            EmailResponsable = emailResponsable ?? throw new ArgumentNullException(nameof(emailResponsable));
            TituloPropuesta = tituloPropuesta;
            Descripcion = descripcion;
            FundamentacionObjetivoMuestra = fundamentacionMuestra;
            LogisticaMuestraTemporal = logisticaMuestraTemporal ?? throw new ArgumentNullException(nameof(logisticaMuestraTemporal));
            Publico = publico;
            RequiereDifusion = requiereDifusion;
            Institucion = institucion;
            EmailInstitucion = emailInstitucion;
            UrlImagenesMuestra = urlImagenes?
              .Where(x => !string.IsNullOrWhiteSpace(x))
              .Select(x => x!)
              .ToList()
              ?? [];

        }
        public void CancelarPropuesta()
        {
            if (Estado == EstadoActividad.Cancelada)
                throw new DomainException(
                    "La propuesta ya está cancelada.");

            CancelarActividad();
        }
        public void MarcarActividadcomoReprogramada()
        {
            if (Estado == EstadoActividad.Cancelada)
                throw new DomainException(
                    "La propuesta ya está cancelada.");
            MarcarComoActividadReprogramada();
        }









    }
}
