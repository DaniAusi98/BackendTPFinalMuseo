using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Entities.Ubicacion
{
    public class DivisionAdministrativa : DomainEntity<string>
    {

        public string Nombre { get; private set; }

        public string Tipo { get; private set; }

        public int Nivel { get; private set; }

        public string PaisId { get; private set; }
        public Pais Pais { get; private set; }

        public string PadreId { get; private set; }
        public DivisionAdministrativa Padre { get; private set; }

        public ICollection<DivisionAdministrativa> Hijas { get; private set; }
            = new List<DivisionAdministrativa>();

        public ICollection<Localidad> Localidades { get; private set; }
            = new List<Localidad>();

        private DivisionAdministrativa() { }

        public DivisionAdministrativa(
            string nombre,
            string tipo,
            int nivel,
            string paisId,
            string padreId = null)
        {
            Id = Guid.NewGuid().ToString();
            Nombre = nombre;
            Tipo = tipo;
            Nivel = nivel;
            PaisId = paisId;
            PadreId = padreId;
        }
    }
}
