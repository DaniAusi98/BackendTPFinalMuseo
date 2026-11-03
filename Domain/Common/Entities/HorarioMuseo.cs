
using Core.Domain.Entities;
using Domain.Common.Exceptions;
using Domain.Common.ValueObjets;

namespace Domain.Common.Entities
{
    public class HorarioAperturaMuseo : DomainEntity<string>
    {
        public List<HorarioApertura> HorariosApertura { get; private set; }

        private readonly List<DiaCierreMuseo> _diasCierre;
        public string ZonaHoraria { get; private set; }

        public IReadOnlyCollection<DiaCierreMuseo> DiasCierre
            => _diasCierre.AsReadOnly();

        protected HorarioAperturaMuseo()
        {
            HorariosApertura = new List<HorarioApertura>();

            _diasCierre = new List<DiaCierreMuseo>();
        }

        public HorarioAperturaMuseo(
            IEnumerable<HorarioApertura> horarios,
            string zonaHoraria,
            IEnumerable<DiaCierreMuseo>? diasCierre = null
            )
        {
            Id = Guid.NewGuid().ToString();

            HorariosApertura = horarios?.ToList()
                ?? throw new ArgumentNullException(nameof(horarios));


            _diasCierre = diasCierre?.ToList()
                ?? new List<DiaCierreMuseo>();

            if (string.IsNullOrWhiteSpace(zonaHoraria))
            {
                throw new ArgumentException("La zona horaria no puede estar vacía.", nameof(zonaHoraria));
            }
            ZonaHoraria = zonaHoraria;
        }

        public void ActualizarHorario(
            IEnumerable<HorarioApertura> nuevosHorario)
        {
            ArgumentNullException.ThrowIfNull(nuevosHorario);

            HorariosApertura = nuevosHorario.ToList();
        }

        public void AgregarDiaCierre(DiaCierreMuseo diaCierre)
        {
            ArgumentNullException.ThrowIfNull(diaCierre);

            ValidarDiaCierre(diaCierre);

            if (_diasCierre.Any(d => d.Id == diaCierre.Id))
            {
                throw new DomainException(
                    "El día de cierre ya existe en el calendario.");
            }

            _diasCierre.Add(diaCierre);
        }

        public void AgregarDiasCierre(
            IEnumerable<DiaCierreMuseo> diasCierre)
        {
            ArgumentNullException.ThrowIfNull(diasCierre);

            var nuevosCierres = diasCierre.ToList();

            if (nuevosCierres.Any(d => d is null))
            {
                throw new DomainException(
                    "La lista de días de cierre no puede contener elementos nulos.");
            }

            foreach (var diaCierre in nuevosCierres)
            {
                ValidarDiaCierre(diaCierre);
            }

            var ids = nuevosCierres.Select(d => d.Id).ToList();

            if (ids.Any(string.IsNullOrWhiteSpace))
            {
                throw new DomainException(
                    "Todos los días de cierre deben tener un identificador válido.");
            }

            if (ids.Distinct().Count() != ids.Count)
            {
                throw new DomainException(
                    "La lista contiene días de cierre con identificadores duplicados.");
            }

            if (ids.Any(id => _diasCierre.Any(d => d.Id == id)))
            {
                throw new DomainException(
                    "Uno o más días de cierre ya existen en el calendario.");
            }

            _diasCierre.AddRange(nuevosCierres);
        }

        private static void ValidarDiaCierre(DiaCierreMuseo diaCierre)
        {
            if (string.IsNullOrWhiteSpace(diaCierre.Id))
            {
                throw new DomainException(
                    "El día de cierre debe tener un identificador válido.");
            }

            if (diaCierre.FechaHasta.HasValue &&
                diaCierre.Fecha >= diaCierre.FechaHasta.Value)
            {
                throw new DomainException(
                    "La fecha de inicio del cierre debe ser anterior a la fecha de fin.");
            }
        }

        public void RemoverDiaCierre(DiaCierreMuseo diaCierre)
        {
            ArgumentNullException.ThrowIfNull(diaCierre);

            _diasCierre.Remove(diaCierre);
        }

        public bool EsDiaApertura(DateTime fecha)
        {
            DayOfWeek diaSemana = fecha.DayOfWeek;
            TimeOnly hora = TimeOnly.FromDateTime(fecha);

            return HorariosApertura.Any(a =>
                a.Dia == diaSemana &&
                hora >= a.Desde &&
                hora <= a.Hasta);
        }

        public bool EsDiaOperativo(DateTime fecha)
        {
            bool tieneHorarioEseDia = HorariosApertura.Any(
                a => a.Dia == fecha.DayOfWeek);

            if (!tieneHorarioEseDia)
                return false;

            var inicioDia = fecha.Date;
            var finDia = inicioDia.AddDays(1);

            return !_diasCierre.Any(d =>
                d.SolapaConFechas(inicioDia, finDia));
        }

        public bool EstaAbierto(DateTime inicio, DateTime fin)
        {
            if (inicio >= fin)
            {
                throw new ArgumentException(
                    "La fecha de inicio debe ser anterior a la fecha de fin.");
            }

            if (inicio.Date != fin.Date)
                return false;

            if (_diasCierre.Any(d =>
                d.SolapaConFechas(inicio, fin)))
            {
                return false;
            }

            DayOfWeek diaSemana = inicio.DayOfWeek;
            TimeOnly horaInicio = TimeOnly.FromDateTime(inicio);
            TimeOnly horaFin = TimeOnly.FromDateTime(fin);

            return HorariosApertura.Any(a =>
                a.Dia == diaSemana &&
                horaInicio >= a.Desde &&
                horaFin <= a.Hasta);
        }

        public IEnumerable<TimeSlot> ObtenerFranjasOperativas(DateOnly fecha)
        {
            DayOfWeek diaSemana = fecha.DayOfWeek;

            var horariosDelDia = HorariosApertura
                .Where(a => a.Dia == diaSemana)
                .ToList();

            if (horariosDelDia.Count == 0)
                return Enumerable.Empty<TimeSlot>();

            var franjasDisponibles = new List<TimeSlot>();

            var inicioDia = fecha.ToDateTime(TimeOnly.MinValue);
            var finDia = inicioDia.AddDays(1);

            // Si existe un cierre que cubre el día completo,
            // no hay franjas operativas para esa fecha.
            if (_diasCierre.Any(d =>
                d.SolapaConFechas(inicioDia, finDia) &&
                d.Fecha <= inicioDia &&
                (d.FechaHasta == null ||
                 d.FechaHasta.Value >= finDia)))
            {
                return Enumerable.Empty<TimeSlot>();
            }

            foreach (var horario in horariosDelDia)
            {
                var horaInicio = fecha.ToDateTime(horario.Desde);
                var horaFin = fecha.ToDateTime(horario.Hasta);

                var cierresFranja = _diasCierre
                    .Where(d => d.SolapaConFechas(horaInicio, horaFin))
                    .OrderBy(d => d.Fecha)
                    .ToList();

                if (cierresFranja.Count == 0)
                {
                    franjasDisponibles.Add(
                        new TimeSlot(horaInicio, horaFin));

                    continue;
                }

                var inicioFranja = horaInicio;

                foreach (var cierre in cierresFranja)
                {
                    var finCierre = cierre.FechaHasta
                        ?? cierre.Fecha.Date.AddDays(1);

                    var inicioCierre = cierre.Fecha > horaInicio
                        ? cierre.Fecha
                        : horaInicio;

                    // Limitar el cierre a la franja horaria actual.
                    if (inicioCierre < horaInicio)
                        inicioCierre = horaInicio;

                    if (finCierre > horaFin)
                        finCierre = horaFin;

                    if (inicioCierre > inicioFranja)
                    {
                        franjasDisponibles.Add(
                            new TimeSlot(inicioFranja, inicioCierre));
                    }

                    if (finCierre > inicioFranja)
                        inicioFranja = finCierre;

                    if (inicioFranja >= horaFin)
                        break;
                }

                if (inicioFranja < horaFin)
                {
                    franjasDisponibles.Add(
                        new TimeSlot(inicioFranja, horaFin));
                }
            }

            return franjasDisponibles;
        }
    }
}
