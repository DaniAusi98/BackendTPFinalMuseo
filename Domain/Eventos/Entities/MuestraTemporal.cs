
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
        public string DescripcionMuestra { get; private set; } = string.Empty;
        public string FundamentacionMuestra { get; private set; } = string.Empty;
        public LogisticaMuestraTemporal LogisticaMuestraTemporal { get; private set; }
        public string? Publico { get; private set; }
        public bool RequiereDifusion { get; private set; }
        public List<string> UrlImagenesMuestra { get; private set; } = [];
        public bool SolicitaFlyer { get; private set; }

        public MuestraTemporal(
            string nombreResponsable,
            Telefono telefonoResponsable,
            Email emailResponsable,
            string tituloPropuesta,
            LogisticaMuestraTemporal logisticaMuestraTemporal,
            bool requiereDifusion,
            IEnumerable<Sala> salas,
            TimeSlot fechaMuestra,
            string? publico = null,
            string? descripcionMuestra = null,
            string? fundamentacionMuestra = null,
            string? institucion = null,
            Email? emailInstitucion = null,
            IEnumerable<string?>? urlImagenes = null,
            IEnumerable<RecursoAsignado>? recursos = null)
            : base(
                tituloPropuesta,
                TipoActividad.MuestraExposicionTemporal,
                fechaMuestra,
                salas: salas,
                recursos: recursos)
        {
            ValidarNombreResponsable(nombreResponsable);
            ValidarTitulo(tituloPropuesta);
            ValidarCampoOpcional(publico, nameof(publico), "El público");
            ValidarCampoOpcional(
                descripcionMuestra,
                nameof(descripcionMuestra),
                "La descripción de la muestra");
            ValidarCampoOpcional(
                fundamentacionMuestra,
                nameof(fundamentacionMuestra),
                "La fundamentación de la muestra");
            ValidarCampoOpcional(
                institucion,
                nameof(institucion),
                "La institución");

            NombreApellidoResponsable = nombreResponsable.Trim();
            TelefonoResponsable = telefonoResponsable
                ?? throw new ArgumentNullException(nameof(telefonoResponsable));
            EmailResponsable = emailResponsable
                ?? throw new ArgumentNullException(nameof(emailResponsable));

            TituloPropuesta = tituloPropuesta.Trim();
            DescripcionMuestra = descripcionMuestra?.Trim() ?? string.Empty;
            FundamentacionMuestra = fundamentacionMuestra?.Trim() ?? string.Empty;

            LogisticaMuestraTemporal = logisticaMuestraTemporal
                ?? throw new ArgumentNullException(nameof(logisticaMuestraTemporal));

            Publico = publico?.Trim();
            RequiereDifusion = requiereDifusion;
            Institucion = institucion?.Trim();
            EmailInstitucion = emailInstitucion;

            UrlImagenesMuestra = urlImagenes?
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x!.Trim())
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
            LogisticaMuestraTemporal logisticaMuestraTemporal,
            string? publico,
            bool requiereDifusion,
            IEnumerable<Sala> salas,
            string? descripcionMuestra = null,
            string? fundamentacionMuestra = null,
            string? institucion = null,
            Email? emailInstitucion = null,
            IEnumerable<string?>? urlImagenes = null,
            IEnumerable<RecursoAsignado>? recursos = null)
        {
            ValidarNombreResponsable(nombreResponsable);
            ValidarTitulo(tituloPropuesta);
            ValidarCampoOpcional(publico, nameof(publico), "El público");
            ValidarCampoOpcional(
                descripcionMuestra,
                nameof(descripcionMuestra),
                "La descripción de la muestra");
            ValidarCampoOpcional(
                fundamentacionMuestra,
                nameof(fundamentacionMuestra),
                "La fundamentación de la muestra");
            ValidarCampoOpcional(
                institucion,
                nameof(institucion),
                "La institución");

            ArgumentNullException.ThrowIfNull(telefonoResponsable);
            ArgumentNullException.ThrowIfNull(emailResponsable);
            ArgumentNullException.ThrowIfNull(logisticaMuestraTemporal);
            ArgumentNullException.ThrowIfNull(salas);

            ActualizarTitulo(tituloPropuesta);
            ActualizarSalas(salas);
            ActualizarRecursos(recursos);

            NombreApellidoResponsable = nombreResponsable.Trim();
            TelefonoResponsable = telefonoResponsable;
            EmailResponsable = emailResponsable;
            TituloPropuesta = tituloPropuesta.Trim();

            DescripcionMuestra = descripcionMuestra?.Trim() ?? string.Empty;
            FundamentacionMuestra = fundamentacionMuestra?.Trim() ?? string.Empty;
            LogisticaMuestraTemporal = logisticaMuestraTemporal;

            Publico = publico?.Trim();
            RequiereDifusion = requiereDifusion;
            Institucion = institucion?.Trim();
            EmailInstitucion = emailInstitucion;

            UrlImagenesMuestra = urlImagenes?
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x!.Trim())
                .ToList()
                ?? [];
        }

        private static void ValidarNombreResponsable(string nombreResponsable)
        {
            if (string.IsNullOrWhiteSpace(nombreResponsable))
            {
                throw new ArgumentException(
                    "El nombre y apellido del responsable de la muestra no puede estar vacío.",
                    nameof(nombreResponsable));
            }
        }

        private static void ValidarTitulo(string tituloPropuesta)
        {
            if (string.IsNullOrWhiteSpace(tituloPropuesta))
            {
                throw new ArgumentException(
                    "El título de la propuesta no puede estar vacío.",
                    nameof(tituloPropuesta));
            }
        }

        private static void ValidarCampoOpcional(
            string? valor,
            string nombreParametro,
            string nombreCampo)
        {
            if (valor is not null && string.IsNullOrWhiteSpace(valor))
            {
                throw new ArgumentException(
                    $"{nombreCampo} no puede estar vacío.",
                    nombreParametro);
            }
        }

        public void CancelarPropuesta()
        {
            if (Estado == EstadoActividad.Cancelada)
            {
                throw new DomainException(
                    "La propuesta ya está cancelada.");
            }

            CancelarActividad();
        }

        public void MarcarActividadcomoReprogramada()
        {
            if (Estado == EstadoActividad.Cancelada)
            {
                throw new DomainException(
                    "La propuesta ya está cancelada.");
            }

            MarcarComoActividadReprogramada();
        }
    }
}
