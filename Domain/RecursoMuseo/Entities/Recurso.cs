using Core.Domain.Entities;

using Domain.Common.Exceptions;

using static Domain.RecursoMuseo.Enums.Enums;

namespace Domain.RecursoMuseo.Entities
{
    public class Recurso : DomainEntity<string>
    {
        public string NombreRecurso { get; private set; }

        public TipoRecurso TipoRecurso { get; private set; }

        public string Descripcion { get; private set; }

        public EstadoRecurso Estado { get; private set; }

        public int CantidadTotal { get; private set; }

        protected Recurso()
        {
        }

        public Recurso(
            string nombreRecurso,
            TipoRecurso tipoRecurso,
            string descripcion,
            int cantidadTotal)
        {
            Id = Guid.NewGuid().ToString();

            ActualizarNombre(nombreRecurso);
            ActualizarTipo(tipoRecurso);
            ActualizarDescripcion(descripcion);
            ActualizarCantidadTotal(cantidadTotal);

            Estado = EstadoRecurso.Disponible;
        }

        public void ActualizarNombre(string nuevoNombre)
        {
            if (string.IsNullOrWhiteSpace(nuevoNombre))
                throw new DomainException("El nombre del recurso es obligatorio.");

            if (nuevoNombre.Length > 200)
                throw new DomainException("El nombre del recurso no puede superar los 200 caracteres.");

            NombreRecurso = nuevoNombre.Trim();
        }

        public void ActualizarTipo(TipoRecurso nuevoTipo)
        {
            if (!Enum.IsDefined(typeof(TipoRecurso), nuevoTipo))
                throw new DomainException("El tipo de recurso no es válido.");

            TipoRecurso = nuevoTipo;
        }

        public void ActualizarDescripcion(string nuevaDescripcion)
        {
            if (string.IsNullOrWhiteSpace(nuevaDescripcion))
                throw new DomainException("La descripción del recurso es obligatoria.");

            if (nuevaDescripcion.Length > 1000)
                throw new DomainException("La descripción no puede superar los 1000 caracteres.");

            Descripcion = nuevaDescripcion.Trim();
        }

        public void ActualizarCantidadTotal(int nuevaCantidad)
        {
            if (nuevaCantidad <= 0)
                throw new DomainException("La cantidad total debe ser mayor a cero.");

            CantidadTotal = nuevaCantidad;
        }

        public void CambiarEstado(EstadoRecurso nuevoEstado)
        {
            if (!Enum.IsDefined(typeof(EstadoRecurso), nuevoEstado))
                throw new DomainException("El estado del recurso no es válido.");

            Estado = nuevoEstado;
        }
    }
}
