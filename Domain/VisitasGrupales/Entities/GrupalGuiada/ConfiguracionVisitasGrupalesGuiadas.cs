using Core.Domain.Entities;
using Domain.Common.Exceptions;
using Domain.VisitasGrupales.ValueObjects;
using Domain.Common.Entities;
using Domain.Common.ValueObjets;

namespace Domain.VisitasGrupales.Entities.GrupalGuiada
{
    public class ConfiguracionVisitasGrupalesGuiadas : DomainEntity<string>
    {
        private readonly List<TurnoVisitaGuiada> _turnos;
        private readonly List<BloqueoVisitasGuiadas> _bloqueos;

        public int CapacidadPorGuia { get; private set; }

        public int CapacidadMaximaPorTurno { get; private set; }

        public int MaximoVisitasSimultaneas { get; private set; }

        public DiasLaboralesMuseo DiasDisponibles { get; private set; }

        public IReadOnlyCollection<TurnoVisitaGuiada> Turnos =>
            _turnos.AsReadOnly();

        public IReadOnlyCollection<BloqueoVisitasGuiadas> Bloqueos =>
            _bloqueos.AsReadOnly();

        protected ConfiguracionVisitasGrupalesGuiadas()
        {
            _turnos = new List<TurnoVisitaGuiada>();
            _bloqueos = new List<BloqueoVisitasGuiadas>();
        }

        public ConfiguracionVisitasGrupalesGuiadas(
            int capacidadPorGuia,
            int capacidadMaxima,
            int maximoVisitasSimultaneas,
            DiasLaboralesMuseo diasDisponibles,
            IReadOnlyCollection<TurnoVisitaGuiada> turnos,
            IReadOnlyCollection<BloqueoVisitasGuiadas>? bloqueos = null)
        {
            Id = Guid.NewGuid().ToString();

            if (capacidadPorGuia <= 0)
                throw new DomainException(
                    "La capacidad por guía debe ser mayor a 0.");

            if (capacidadMaxima <= 0)
                throw new DomainException(
                    "La capacidad máxima por turno debe ser mayor a 0.");

            if (capacidadMaxima < capacidadPorGuia)
                throw new DomainException(
                    "La capacidad máxima no puede ser menor a la capacidad por guía.");

            if (maximoVisitasSimultaneas <= 0)
                throw new DomainException(
                    "El máximo de visitas simultáneas debe ser mayor a 0.");

            CapacidadPorGuia = capacidadPorGuia;
            CapacidadMaximaPorTurno = capacidadMaxima;
            MaximoVisitasSimultaneas = maximoVisitasSimultaneas;

            DiasDisponibles = diasDisponibles
                ?? throw new ArgumentNullException(nameof(diasDisponibles));

            _turnos = turnos?.ToList()
                ?? throw new ArgumentNullException(nameof(turnos));

            _bloqueos = bloqueos?.ToList()
                ?? new List<BloqueoVisitasGuiadas>();

            ValidarTurnosNoSeSolapen();
        }

        public void ActualizarCapacidad(
            int capacidadPorGuia,
            int capacidadMaxima,
            int maximoVisitasSimultaneas)
        {
            if (capacidadPorGuia <= 0)
                throw new DomainException(
                    "La capacidad por guía debe ser mayor a 0.");

            if (capacidadMaxima <= 0)
                throw new DomainException(
                    "La capacidad máxima por turno debe ser mayor a 0.");

            if (capacidadMaxima < capacidadPorGuia)
                throw new DomainException(
                    "La capacidad máxima no puede ser menor a la capacidad por guía.");

            if (maximoVisitasSimultaneas <= 0)
                throw new DomainException(
                    "El máximo de visitas simultáneas debe ser mayor a 0.");

            CapacidadPorGuia = capacidadPorGuia;
            CapacidadMaximaPorTurno = capacidadMaxima;
            MaximoVisitasSimultaneas = maximoVisitasSimultaneas;
        }

        public void ActualizarDiasDisponibles(
            DiasLaboralesMuseo diasDisponibles,
            CalendarioMuseo calendarioMuseo)
        {
            if (diasDisponibles == null)
                throw new ArgumentNullException(nameof(diasDisponibles));

            if (calendarioMuseo == null)
                throw new ArgumentNullException(nameof(calendarioMuseo));

            var diasNoLaborales = diasDisponibles.Dias
                .Where(d => !calendarioMuseo.DiasApertura.Dias.Contains(d))
                .ToList();

            if (diasNoLaborales.Any())
            {
                throw new DomainException(
                    $"Los días {string.Join(", ", diasNoLaborales)} " +
                    $"no son días laborales del museo. " +
                    $"Solo se pueden configurar visitas en: " +
                    $"{string.Join(", ", calendarioMuseo.DiasApertura.Dias)}");
            }

            DiasDisponibles = diasDisponibles;
        }

        public void AgregarTurno(TurnoVisitaGuiada turno)
        {
            if (turno == null)
                throw new ArgumentNullException(nameof(turno));

            if (_turnos.Any(t => t.SeSolapaCon(turno)))
                throw new DomainException(
                    "El turno se solapa con un turno existente.");

            _turnos.Add(turno);
        }

        public void RemoverTurno(TurnoVisitaGuiada turno)
        {
            if (turno == null)
                throw new ArgumentNullException(nameof(turno));

            _turnos.Remove(turno);
        }

        public void ActualizarTurnos(
            IReadOnlyCollection<TurnoVisitaGuiada> nuevosTurnos)
        {
            if (nuevosTurnos == null)
                throw new ArgumentNullException(nameof(nuevosTurnos));

            if (!nuevosTurnos.Any())
                throw new DomainException(
                    "Debe existir al menos un turno disponible.");

            _turnos.Clear();
            _turnos.AddRange(nuevosTurnos);

            ValidarTurnosNoSeSolapen();
        }

        public void AgregarBloqueo(BloqueoVisitasGuiadas bloqueo)
        {
            if (bloqueo == null)
                throw new ArgumentNullException(nameof(bloqueo));

            if (_bloqueos.Any(b =>
                b.SolapaConPeriodo(
                    bloqueo.FechaDesde,
                    bloqueo.FechaHasta)))
            {
                throw new DomainException(
                    "El bloqueo se solapa con un bloqueo existente.");
            }

            _bloqueos.Add(bloqueo);
        }

        public void RemoverBloqueo(BloqueoVisitasGuiadas bloqueo)
        {
            if (bloqueo == null)
                throw new ArgumentNullException(nameof(bloqueo));

            _bloqueos.Remove(bloqueo);
        }

        public bool EstaDisponibleEnFecha(DateTime fecha)
        {
            if (!EsDiaDisponible(fecha.DayOfWeek))
                return false;

            return !_bloqueos.Any(b => b.SolapaConFecha(fecha));
        }

        public bool EsDiaDisponible(DayOfWeek dia)
        {
            return DiasDisponibles.EsDiaLaboral(dia);
        }

        public int CalcularCapacidadDisponible(int guiasDisponibles)
        {
            if (guiasDisponibles <= 0)
                return 0;

            return Math.Min(
                guiasDisponibles * CapacidadPorGuia,
                CapacidadMaximaPorTurno);
        }

        private void ValidarTurnosNoSeSolapen()
        {
            for (int i = 0; i < _turnos.Count; i++)
            {
                for (int j = i + 1; j < _turnos.Count; j++)
                {
                    if (_turnos[i].SeSolapaCon(_turnos[j]))
                    {
                        throw new DomainException(
                            $"Los turnos {_turnos[i].HoraInicio}-" +
                            $"{_turnos[i].HoraFin} y " +
                            $"{_turnos[j].HoraInicio}-" +
                            $"{_turnos[j].HoraFin} se solapan.");
                    }
                }
            }
        }
    }
}