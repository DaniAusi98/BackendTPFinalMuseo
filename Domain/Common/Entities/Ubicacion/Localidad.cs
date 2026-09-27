
using Core.Domain.Entities;

namespace Domain.Common.Entities.Ubicacion
{
    public class Localidad : DomainEntity<string>
    {

        public string Nombre { get; private set; }

        public string Tipo { get; private set; }

        public string CodigoPostal { get; private set; }

        public double? Latitud { get; private set; }

        public double? Longitud { get; private set; }

        public string DivisionAdministrativaId { get; private set; }

        public DivisionAdministrativa DivisionAdministrativa { get; private set; }

        private Localidad() { }

        public Localidad(
            string nombre,
            string tipo,
            string divisionAdministrativaId = null,
            string codigoPostal = null,
            double? latitud = null,
            double? longitud = null)
        {
            Id = Guid.NewGuid().ToString();
            Nombre = nombre;
            Tipo = tipo;
            DivisionAdministrativaId = divisionAdministrativaId;
            CodigoPostal = codigoPostal;
            Latitud = latitud;
            Longitud = longitud;
        }
    }
}
