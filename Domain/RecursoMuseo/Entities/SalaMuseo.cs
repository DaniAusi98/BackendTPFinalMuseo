using Core.Domain.Entities;

using Domain.Common.Exceptions;

using static Domain.RecursoMuseo.Enums.Enums;

namespace Domain.RecursoMuseo.Entities
{
    public class Sala : DomainEntity<string>
    {
        public string Nombre { get; private set; }

        public EstadoSala EstadoSala { get; private set; }

        public string CodigoSala { get; private set; }

        public TipoSala TipoSala { get; private set; }

        public int Capacidad { get; private set; }

        public UbicacionSala Ubicacion { get; private set; }

        protected Sala()
        {
        }

        public Sala(
            string nombre,
            TipoSala tipoSala,
            int capacidad,
            UbicacionSala ubicacion,
            string codigoSala)
        {
            Id = Guid.NewGuid().ToString();

            ActualizarNombre(nombre);
            ActualizarTipoSala(tipoSala);
            ActualizarCapacidad(capacidad);
            ActualizarUbicacion(ubicacion);
            ActualizarCodigoSala(codigoSala);
            EstadoSala = EstadoSala.Activa; // Por defecto, la sala se crea como activa
        }

        public void ActualizarNombre(string nuevoNombre)
        {
            if (string.IsNullOrWhiteSpace(nuevoNombre))
                throw new DomainException("El nombre de la sala es obligatorio.");

            if (nuevoNombre.Length > 100)
                throw new DomainException("El nombre de la sala no puede superar los 100 caracteres.");

            Nombre = nuevoNombre.Trim();
        }

        public void ActualizarTipoSala(TipoSala nuevoTipo)
        {
            if (!Enum.IsDefined(typeof(TipoSala), nuevoTipo))
                throw new DomainException("El tipo de sala no es válido.");

            TipoSala = nuevoTipo;
        }

        public void ActualizarCapacidad(int nuevaCapacidad)
        {
            if (nuevaCapacidad <= 0)
                throw new DomainException("La capacidad debe ser mayor a cero.");

            Capacidad = nuevaCapacidad;
        }

        public void ActualizarUbicacion(UbicacionSala nuevaUbicacion)
        {
            if (!Enum.IsDefined(typeof(UbicacionSala), nuevaUbicacion))
                throw new DomainException("La ubicación de la sala no es válida.");

            Ubicacion = nuevaUbicacion;
        }

        public void ActualizarCodigoSala(string nuevoCodigo)
        {
            if (string.IsNullOrWhiteSpace(nuevoCodigo))
                throw new DomainException("El código de la sala es obligatorio.");

            if (nuevoCodigo.Length > 50)
                throw new DomainException("El código de la sala no puede superar los 50 caracteres.");

            CodigoSala = nuevoCodigo.Trim();
        }
        

        public void CambiarEstado(EstadoSala nuevoEstado)
        {
            if (!Enum.IsDefined(typeof(EstadoSala), nuevoEstado))
                throw new DomainException("El estado de la sala no es válido.");

            // Si el estado es el mismo, no hacemos nada (operación idempotente).
            if (EstadoSala == nuevoEstado)
                return;

            EstadoSala = nuevoEstado;
        }
    }
}
