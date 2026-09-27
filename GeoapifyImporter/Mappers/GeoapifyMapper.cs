using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common.Entities.Ubicacion;
using GeoapifyImporter.Models;
using global::GeoapifyImporter.Models;

namespace GeoapifyImporter.Mappers
{



    public class GeoapifyMapper
    {
        private readonly Dictionary<string, Pais> _paises = new();
        private readonly Dictionary<string, DivisionAdministrativa> _divisiones = new();

        public (Pais Pais, DivisionAdministrativa Provincia, DivisionAdministrativa? Departamento, Localidad Localidad)
            Map(GeoapifyLocality localidad)
        {
            if (localidad.Address == null)
                throw new InvalidOperationException(
                    $"La localidad '{localidad.Name}' no tiene información de dirección.");

            var address = localidad.Address;

            // =========================
            // PAÍS
            // =========================

            var codigoPais = address.CountryCode?.ToUpperInvariant();

            if (string.IsNullOrWhiteSpace(codigoPais))
                throw new InvalidOperationException(
                    $"La localidad '{localidad.Name}' no tiene código de país.");

            if (!_paises.TryGetValue(codigoPais, out var pais))
            {
                pais = new Pais(
                    address.Country ?? "Desconocido",
                    codigoPais);

                _paises[codigoPais] = pais;
            }

            // =========================
            // PROVINCIA / ESTADO
            // =========================

            var nombreProvincia = address.State;

            if (string.IsNullOrWhiteSpace(nombreProvincia))
                throw new InvalidOperationException(
                    $"La localidad '{localidad.Name}' no tiene provincia/estado.");

            var claveProvincia = $"{pais.Id}|{nombreProvincia}";

            if (!_divisiones.TryGetValue(claveProvincia, out var provincia))
            {
                provincia = new DivisionAdministrativa(
                    nombreProvincia,
                    "state",
                    1,
                    pais.Id);

                _divisiones[claveProvincia] = provincia;
            }

            // =========================
            // DEPARTAMENTO / DIVISIÓN 2
            // =========================

            DivisionAdministrativa? departamento = null;

            if (!string.IsNullOrWhiteSpace(address.StateDistrict))
            {
                var nombreDepartamento = address.StateDistrict;

                var claveDepartamento =
                    $"{provincia.Id}|{nombreDepartamento}";

                if (!_divisiones.TryGetValue(claveDepartamento, out departamento))
                {
                    departamento = new DivisionAdministrativa(
                        nombreDepartamento,
                        "state_district",
                        2,
                        pais.Id,
                        provincia.Id);

                    _divisiones[claveDepartamento] = departamento;
                }
            }

            // =========================
            // LOCALIDAD
            // =========================

            var divisionId = departamento?.Id ?? provincia.Id;

            var localidadDomain = new Localidad(
                localidad.Name,
                localidad.Type ?? "unknown",
                divisionId);

            return (
                pais,
                provincia,
                departamento,
                localidadDomain);
        }

        public IReadOnlyCollection<Pais> ObtenerPaises()
        {
            return _paises.Values;
        }

        public IReadOnlyCollection<DivisionAdministrativa> ObtenerDivisiones()
        {
            return _divisiones.Values;
        }
    }

}
