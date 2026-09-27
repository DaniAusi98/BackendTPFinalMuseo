using Core.Domain.Entities;
using Domain.Common.Exceptions;
using Domain.RecursoMuseo.Entities;

namespace Domain.ActividadMuseo.Entities
{
    public class RecursoAsignado:DomainEntity<string>
    {
        public string ActividadId { get; private set; }
        public ActividadMuseo Actividad { get; private set; }

        public string RecursoId { get; private set; }
        public Recurso Recurso { get; private set; }

        public int CantidadAsignada { get; private set; }

        public DateTime Inicio { get; private set; }
        public DateTime Fin { get; private set; }

        public RecursoAsignado(string recursoId, int cantidadAsignada, DateTime inicio, DateTime fin)
        {
            Id = Guid.NewGuid().ToString();

            if (string.IsNullOrEmpty(recursoId))
                throw new DomainException("El recurso es obligatorio.");

            if (cantidadAsignada <= 0)
                throw new DomainException("La cantidad asignada debe ser mayor a cero.");
            
            if (inicio >= fin)
                throw new DomainException("La fecha de inicio debe ser anterior a la fecha de fin.");

            RecursoId = recursoId;
            CantidadAsignada = cantidadAsignada;
            Inicio = inicio;
            Fin = fin;
        }

        public void ActualizarCantidad(int nuevaCantidad)
        {
            if (nuevaCantidad <= 0)
                throw new DomainException("La cantidad asignada debe ser mayor a cero.");

            CantidadAsignada = nuevaCantidad;
        }

        private RecursoAsignado() { }
    }
}

