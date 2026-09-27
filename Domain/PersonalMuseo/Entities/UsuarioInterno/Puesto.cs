using Core.Domain.Entities;

using System;
using System.Collections.Generic;
using Domain.Common.Exceptions;

namespace Domain.PersonalMuseo.Entities.UsuarioInterno
{
    public class Puesto : DomainEntity<string>
    {
        public string Nombre { get; private set; }

        public List<AreaPuesto> AreaPuestos { get; private set; } = new List<AreaPuesto>();

        public Puesto()
        {
        }

        public Puesto(string nombre)
        {
            Id = Guid.NewGuid().ToString();

            SetNombre(nombre);
        }

        public void SetNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new DomainException("El nombre del puesto no puede estar vacío.");

            Nombre = nombre.Trim();
        }
    }
}
