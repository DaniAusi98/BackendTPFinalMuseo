using Core.Domain.Entities;
using Domain.Common.Entities;
using Domain.Common.Exceptions;
using Domain.Common.ValueObjets;
using Domain.VisitasGrupales.ValueObjects;

namespace Domain.VisitasGrupales.Entities
{
    public class ConfiguracionHorarioAutoguiada : DomainEntity<string>
    {
        private readonly List<BloqueoAutoguiada> _bloqueos;

        public DiasLaboralesMuseo DiasDisponibles { get; private set; }

        public Horario HorarioDisponibleVisitaAutoguiadas { get; private set; }

        public int CapacidadMaximaPorGrupo { get; private set; }

        /// <summary>
        /// Tiempo que permanece una visita autoguiada dentro del museo.
        /// </summary>
        public TimeSpan DuracionVisita { get; private set; }

        /// <summary>
        /// Tiempo entre una posible entrada y la siguiente.
        /// Ejemplo: una entrada cada 30 minutos.
        /// </summary>
        public TimeSpan IntervaloReservas { get; private set; }

        /// <summary>
        /// Cantidad máxima de grupos que pueden permanecer
        /// simultáneamente dentro del museo.
        /// </summary>
        public int VisitasSimultaneasMaximas { get; private set; }

        public IReadOnlyCollection<BloqueoAutoguiada> Bloqueos
            => _bloqueos.AsReadOnly();

        protected ConfiguracionHorarioAutoguiada()
        {
            _bloqueos = new List<BloqueoAutoguiada>();
        }

        public ConfiguracionHorarioAutoguiada(
            int capacidadMaximaPorGrupo,
            DiasLaboralesMuseo diasDisponibles,
            Horario horarioDisponibleVisitaAutoguiadas,
            CalendarioMuseo calendario,
            TimeSpan duracionVisita,
            TimeSpan intervaloReservas,
            int visitasSimultaneasMaximas,
            IReadOnlyCollection<BloqueoAutoguiada>? bloqueos = null)
        {
            Id = Guid.NewGuid().ToString();

            if (calendario == null)
                throw new ArgumentNullException(nameof(calendario));

            if (capacidadMaximaPorGrupo <= 0)
                throw new DomainException(
                    "La capacidad máxima por grupo debe ser mayor a 0.");

            if (duracionVisita <= TimeSpan.Zero)
                throw new DomainException(
                    "La duración de la visita debe ser mayor a 0.");

            if (intervaloReservas <= TimeSpan.Zero)
                throw new DomainException(
                    "El intervalo entre reservas debe ser mayor a 0.");

            if (intervaloReservas > duracionVisita)
                throw new DomainException(
                    "El intervalo entre reservas no puede ser mayor que la duración de la visita.");

            if (visitasSimultaneasMaximas <= 0)
                throw new DomainException(
                    "La cantidad de visitas simultáneas debe ser mayor a 0.");

            if (diasDisponibles == null)
                throw new ArgumentNullException(nameof(diasDisponibles));

            if (horarioDisponibleVisitaAutoguiadas == null)
                throw new ArgumentNullException(
                    nameof(horarioDisponibleVisitaAutoguiadas));

            ValidarHorarioDentroDelCalendario(
                horarioDisponibleVisitaAutoguiadas,
                calendario);

            CapacidadMaximaPorGrupo = capacidadMaximaPorGrupo;

            DiasDisponibles = diasDisponibles;

            HorarioDisponibleVisitaAutoguiadas =
                horarioDisponibleVisitaAutoguiadas;

            DuracionVisita = duracionVisita;

            IntervaloReservas = intervaloReservas;

            VisitasSimultaneasMaximas = visitasSimultaneasMaximas;

            _bloqueos =
                bloqueos?.ToList()
                ?? new List<BloqueoAutoguiada>();
        }

        public void ActualizarHorarioDisponible(
            Horario horario,
            CalendarioMuseo calendario)
        {
            if (horario == null)
                throw new ArgumentNullException(nameof(horario));

            if (calendario == null)
                throw new ArgumentNullException(nameof(calendario));

            ValidarHorarioDentroDelCalendario(
                horario,
                calendario);

            HorarioDisponibleVisitaAutoguiadas = horario;
        }

        public void ActualizarCapacidadMaxima(
            int capacidadMaximaPorGrupo)
        {
            if (capacidadMaximaPorGrupo <= 0)
                throw new DomainException(
                    "La capacidad máxima por grupo debe ser mayor a 0.");

            CapacidadMaximaPorGrupo = capacidadMaximaPorGrupo;
        }

        public void ActualizarDuracionVisita(
            TimeSpan duracionVisita)
        {
            if (duracionVisita <= TimeSpan.Zero)
                throw new DomainException(
                    "La duración de la visita debe ser mayor a 0.");

            if (IntervaloReservas > duracionVisita)
                throw new DomainException(
                    "La duración de la visita no puede ser menor que el intervalo entre reservas.");

            DuracionVisita = duracionVisita;
        }

        public void ActualizarIntervaloReservas(
            TimeSpan intervaloReservas)
        {
            if (intervaloReservas <= TimeSpan.Zero)
                throw new DomainException(
                    "El intervalo entre reservas debe ser mayor a 0.");

            if (intervaloReservas > DuracionVisita)
                throw new DomainException(
                    "El intervalo entre reservas no puede ser mayor que la duración de la visita.");

            IntervaloReservas = intervaloReservas;
        }

        public void ActualizarVisitasSimultaneas(
            int visitasSimultaneasMaximas)
        {
            if (visitasSimultaneasMaximas <= 0)
                throw new DomainException(
                    "La cantidad de visitas simultáneas debe ser mayor a 0.");

            VisitasSimultaneasMaximas =
                visitasSimultaneasMaximas;
        }

        public void ActualizarDiasDisponibles(
            DiasLaboralesMuseo diasDisponibles,
            CalendarioMuseo calendario)
        {
            if (diasDisponibles == null)
                throw new ArgumentNullException(nameof(diasDisponibles));

            if (calendario == null)
                throw new ArgumentNullException(nameof(calendario));

            foreach (var dia in diasDisponibles.Dias)
            {
                if (!calendario.DiasApertura.Dias.Contains(dia))
                {
                    throw new DomainException(
                        $"El día {dia} no está dentro de los días de apertura del museo.");
                }
            }

            DiasDisponibles = diasDisponibles;
        }

        public void AgregarBloqueo(
            BloqueoAutoguiada bloqueo)
        {
            if (bloqueo == null)
                throw new ArgumentNullException(nameof(bloqueo));

            if (_bloqueos.Any(
                b => b.SolapaConPeriodo(
                    bloqueo.FechaDesde,
                    bloqueo.FechaHasta)))
            {
                throw new DomainException(
                    "El bloqueo se solapa con un bloqueo existente.");
            }

            _bloqueos.Add(bloqueo);
        }

        public void RemoverBloqueo(
            BloqueoAutoguiada bloqueo)
        {
            if (bloqueo == null)
                throw new ArgumentNullException(nameof(bloqueo));

            _bloqueos.Remove(bloqueo);
        }

        public bool EstaDisponibleEnFecha(DateTime fecha)
        {
            if (!DiasDisponibles.EsDiaLaboral(fecha.DayOfWeek))
                return false;

            return !_bloqueos.Any(
                b => b.SolapaConFecha(fecha));
        }

        public int CalcularCapacidadMaximaSimultanea()
        {
            return CapacidadMaximaPorGrupo *
                   VisitasSimultaneasMaximas;
        }

        /// <summary>
        /// Genera los horarios posibles de entrada.
        ///
        /// La duración de cada TimeSlot representa la permanencia
        /// completa de la visita.
        ///
        /// Ejemplo:
        /// DuracionVisita = 1 hora
        /// IntervaloReservas = 30 minutos
        ///
        /// 09:00 - 10:00
        /// 09:30 - 10:30
        /// 10:00 - 11:00
        /// 10:30 - 11:30
        /// </summary>
        public IEnumerable<TimeSlot> GenerarSlotsDisponibles(
            DateOnly fecha)
        {
            var fechaDateTime =
                fecha.ToDateTime(TimeOnly.MinValue);

            if (!EstaDisponibleEnFecha(fechaDateTime))
                return Enumerable.Empty<TimeSlot>();

            var slots = new List<TimeSlot>();

            var horaActual =
                fecha.ToDateTime(
                    HorarioDisponibleVisitaAutoguiadas.HoraInicio);

            var horaFin =
                fecha.ToDateTime(
                    HorarioDisponibleVisitaAutoguiadas.HoraFin);

            while (horaActual.Add(DuracionVisita) <= horaFin)
            {
                var finVisita =
                    horaActual.Add(DuracionVisita);

                slots.Add(
                    new TimeSlot(
                        horaActual,
                        finVisita));

                horaActual =
                    horaActual.Add(IntervaloReservas);
            }

            return slots;
        }

        private static void ValidarHorarioDentroDelCalendario(
            Horario horario,
            CalendarioMuseo calendario)
        {
            if (horario.HoraInicio <
                    calendario.HorarioApertura.HoraInicio ||
                horario.HoraFin >
                    calendario.HorarioApertura.HoraFin)
            {
                throw new DomainException(
                    "El horario de visitas debe estar dentro del horario de apertura del museo.");
            }
        }
    }
}