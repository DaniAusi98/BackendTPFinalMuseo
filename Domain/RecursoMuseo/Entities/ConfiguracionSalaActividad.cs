using Core.Domain.Entities;
using Domain.Common.Exceptions;
using static Domain.ActividadMuseo.Enums.Enums;

namespace Domain.RecursoMuseo.Entities
{
    /// <summary>
    /// Configuración de qué tipos de actividades puede realizar una sala.
    /// Define las políticas de uso de cada sala de forma centralizada y parametrizable.
    /// </summary>
    public class ConfiguracionSalaActividad : DomainEntity<string>
    {
        /// <summary>
        /// ID de la sala a la que pertenece esta configuración
        /// </summary>
        public string SalaId { get; private set; }

        /// <summary>
        /// Tipo de actividad que puede realizar la sala
        /// </summary>
        public TipoActividad TipoActividad { get; private set; }

        /// <summary>
        /// Indica si la sala está habilitada para este tipo de actividad
        /// </summary>
        public bool Habilitada { get; private set; }

        /// <summary>
        /// Capacidad máxima específica para este tipo de actividad (opcional)
        /// Si es null, se usa la capacidad general de la sala
        /// </summary>
        public int? CapacidadMaxima { get; private set; }

        protected ConfiguracionSalaActividad()
        {
        }

        public ConfiguracionSalaActividad(
            string salaId,
            TipoActividad tipoActividad,
            bool habilitada,
            int? capacidadMaxima = null)
        {
            if (string.IsNullOrWhiteSpace(salaId))
                throw new DomainException("El ID de la sala es obligatorio.");

            if (!Enum.IsDefined(typeof(TipoActividad), tipoActividad))
                throw new DomainException("El tipo de actividad no es válido.");

            if (capacidadMaxima is not null && capacidadMaxima <= 0)
                throw new DomainException("La capacidad máxima debe ser mayor a cero.");

            Id = Guid.NewGuid().ToString();
            SalaId = salaId;
            TipoActividad = tipoActividad;
            Habilitada = habilitada;
            CapacidadMaxima = capacidadMaxima;
        }

        public void ActualizarHabilitacion(bool habilitada)
        {
            Habilitada = habilitada;
        }

        public void ActualizarCapacidadMaxima(int? nuevaCapacidad)
        {
            if (nuevaCapacidad is not null && nuevaCapacidad <= 0)
                throw new DomainException("La capacidad máxima debe ser mayor a cero.");

            CapacidadMaxima = nuevaCapacidad;
        }

        /// <summary>
        /// Verifica si la sala está disponible para esta actividad
        /// </summary>
        public bool EstaDisponiblePara(TipoActividad tipoActividad)
        {
            return Habilitada && TipoActividad == tipoActividad;
        }
    }
}
