using Core.Domain.Entities;
using Domain.Common.Exceptions;
using Domain.Common.ValueObjets;
using Domain.RecursoMuseo.Entities;
using static Domain.ActividadMuseo.Enums.Enums;

namespace Domain.ActividadMuseo.Entities
{
    public class ActividadMuseo : DomainEntity<string>
    {
        // --- PROPIEDADES DE NEGOCIO ---
        public string Titulo { get; private set; }

        public TipoActividad TipoActividad { get; private set; }


        /// <summary>
        /// Almacena el bloque de tiempo base original (contiene DateTime Start y DateTime End).
        /// </summary>
        public TimeSlot Horario { get; private set; }
        public EstadoActividad Estado { get; private set; }
        public int? CantidadPersonas { get; private set; }

        // --- RELACIONES AGREGADAS ---
        public List<Sala> Salas { get; private set; } = [];
        public List<RecursoAsignado> Recursos { get; private set; } = [];

        // --- SISTEMA DE RECURRENCIA LIMPIO ---
        public bool EsRecurrencia => !string.IsNullOrWhiteSpace(RRule);

        /// <summary>
        /// Cadena estándar iCalendar (RFC 5546). Ejemplo: "FREQ=WEEKLY;BYDAY=TU,TH"
        /// Si es nulo o vacío, se trata de una actividad de instancia única.
        /// </summary>
        public string? RRule { get; private set; }

        // Encapsulamiento estricto de la colección de excepciones de fechas (EXDATE)
        private readonly List<ActividadException> _exceptions = [];
        public IReadOnlyCollection<ActividadException> Exceptions => _exceptions.AsReadOnly();

        // --- CONSTRUCTOR DE DOMINIO ---
        public ActividadMuseo(
            string tituloActividad,
            TipoActividad tipo,
            int? cantidadAsistentes,
            TimeSlot horario,
            IEnumerable<Sala> salas = null,
            IEnumerable<RecursoAsignado> recursos = null,
            string rrule = null
        )
        {
            if (string.IsNullOrWhiteSpace(tituloActividad))
                throw new ArgumentException("El titulo de la actividad  debe ser valido.", nameof(tituloActividad));
            if (cantidadAsistentes is not null && cantidadAsistentes <= 0)
                throw new DomainException("La cantidad de personas debe ser mayor a cero.");

            if (horario is null)
                throw new DomainException("El horario no puede ser nulo.");

            ValidarSalasYRecursos(salas, recursos);

            Id = Guid.NewGuid().ToString();
            Titulo = tituloActividad;
            TipoActividad = tipo;
            CantidadPersonas = cantidadAsistentes;
            Estado = EstadoActividad.Activa;
            Horario = horario;

            if (salas is not null)
                Salas.AddRange(salas);
            if (recursos is not null)
                Recursos.AddRange(recursos);

            if (!string.IsNullOrWhiteSpace(rrule))
                RRule = rrule;
        }

        // Constructor requerido por Entity Framework Core
        protected ActividadMuseo() { }

        public void ActualizarTitulo(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new DomainException("El título no puede estar vacío.");

            Titulo = titulo;
        }

        // --- MÉTODOS DE NEGOCIO PARA RECURRENCIA Y EXCEPCIONES ---

        public void ActualizarExceptions(IEnumerable<ActividadException>? exceptions)
        {
            _exceptions.Clear();

            if (exceptions is not null)
                _exceptions.AddRange(exceptions);
        }

        // --- COMPORTAMIENTOS Y VALIDACIONES DE LA ENTIDAD ---

        private static void ValidarSalasYRecursos(IEnumerable<Sala> salas, IEnumerable<RecursoAsignado> recursos)
        {
            if (salas is null || !salas.Any())
                throw new DomainException("La actividad debe tener al menos una sala.");
            if (salas is not null)
            {
                if (salas.Any(s => s is null))
                    throw new DomainException("No puede haber salas nulas.");
                if (salas.GroupBy(s => s.Id).Any(g => g.Count() > 1))
                    throw new DomainException("No puede haber salas repetidas.");
            }

            if (recursos is not null)
            {
                if (recursos.Any(r => r is null))
                    throw new DomainException("No puede haber recursos nulos.");
                if (recursos.GroupBy(r => r.RecursoId).Any(g => g.Count() > 1))
                    throw new DomainException("No puede haber recursos repetidos.");
            }
        }

        public void CambiarTipoActividad(TipoActividad nuevoTipo) => TipoActividad = nuevoTipo;

        public void ActualizarSalas(IEnumerable<Sala>? salas)
        {
            if (salas is null)
                throw new DomainException("La actividad debe tener al menos una sala.");

            var listaSalas = salas.ToList();

            if (listaSalas.Count == 0)
                throw new DomainException("La actividad debe tener al menos una sala.");

            if (listaSalas.Any(s => s is null))
                throw new DomainException("No puede haber salas nulas.");

            if (listaSalas.GroupBy(s => s.Id).Any(g => g.Count() > 1))
                throw new DomainException("No puede haber salas repetidas.");

            Salas.Clear();
            Salas.AddRange(listaSalas);
        }

        public void ActualizarRecursos(IEnumerable<RecursoAsignado>? recursos)
        {
            if (recursos?.Any(r => r is null) == true)
                throw new DomainException("No puede haber recursos nulos.");

            if (recursos?.GroupBy(r => r.RecursoId).Any(g => g.Count() > 1) == true)
                throw new DomainException("No puede haber recursos repetidos.");

            Recursos.Clear();

            if (recursos is not null)
                Recursos.AddRange(recursos);
        }

        public void CancelarActividad()
        {
            if (Estado == EstadoActividad.Cancelada)
                throw new DomainException("La actividad ya está cancelada.");
            Estado = EstadoActividad.Cancelada;
        }

        public void MarcarComoActividadReprogramada()
        {
            if (Estado == EstadoActividad.Cancelada || Estado == EstadoActividad.Reprogramada)
                throw new DomainException("No se puede reprogramar una actividad cancelada o ya reprogramada.");
            Estado = EstadoActividad.Reprogramada;
        }

        public void Activar()
        {
            if (Estado == EstadoActividad.Cancelada || Estado == EstadoActividad.Reprogramada)
                throw new DomainException("No se puede activar una actividad cancelada o reprogramada.");
            Estado = EstadoActividad.Activa;
        }

        public void CambiarCantidadAsistentes(int? nuevaCantidad)
        {
            if (nuevaCantidad is not null && nuevaCantidad <= 0)
                throw new DomainException("La cantidad de personas debe ser mayor a cero.");
            CantidadPersonas = nuevaCantidad;
        }

        public void AsignarSalas(IEnumerable<Sala> salas)
        {
            ArgumentNullException.ThrowIfNull(salas);
            if (salas.Any(s => s is null))
                throw new DomainException("No puede haber salas nulas.");
            if (salas.GroupBy(s => s.Id).Any(g => g.Count() > 1))
                throw new DomainException("No puede haber salas repetidas.");
            Salas.Clear();
            Salas.AddRange(salas);
        }



        public void AsignarRecursos(IEnumerable<RecursoAsignado> recursos)
        {
            ArgumentNullException.ThrowIfNull(recursos);
            if (recursos.Any(r => r is null))
                throw new DomainException("No puede haber recursos nulos.");
            if (recursos.GroupBy(r => r.RecursoId).Any(g => g.Count() > 1))
                throw new DomainException("No puede haber recursos repetidos.");
            Recursos.Clear();
            Recursos.AddRange(recursos);
        }

        public void AsignarTimeSlots(TimeSlot horario)
        {
            ArgumentNullException.ThrowIfNull(horario);
            Horario = horario;
        }
        public void ActualizarRRule(string? nuevaRRule)
        {
            if (!string.IsNullOrWhiteSpace(nuevaRRule))
            {
                RRule = nuevaRRule;
            }
            else
            {
                RRule = null;
            }

            _exceptions.Clear();
        }
    }
}
