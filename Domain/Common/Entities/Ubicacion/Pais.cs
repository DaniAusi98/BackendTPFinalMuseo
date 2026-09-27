using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Entities.Ubicacion
{
    public class Pais:DomainEntity<string>
    {

        public string Nombre { get; private set; }

        public string Codigo { get; private set; }

        public ICollection<DivisionAdministrativa> Divisiones { get; private set; }
            = new List<DivisionAdministrativa>();

        private Pais() { }

        public Pais(string nombre, string codigo)
        {
            Id = Guid.NewGuid().ToString();
            Nombre = nombre;
            Codigo = codigo;
        }
    }
}
