using Core.Domain.Entities;
using Domain.Common.ValueObjets;
using Domain.Common.Exceptions;

namespace Domain.Common.Entities
{
    public class CalendarioMuseo : DomainEntity<string>
    {
        public Horario HorarioApertura { get; private set; }
        public DiasLaboralesMuseo DiasApertura { get; private set; }

        private readonly List<DiaCierreMuseo> _diasCierre;
        public IReadOnlyCollection<DiaCierreMuseo> DiasCierre
            => _diasCierre.AsReadOnly();
        protected CalendarioMuseo() 
        {
            _diasCierre = new List<DiaCierreMuseo>();
        }

        public CalendarioMuseo(
            Horario horario,
            DiasLaboralesMuseo diasLaborales,
            IReadOnlyCollection<DiaCierreMuseo> diasCierre)
        {
            Id = Guid.NewGuid().ToString();

            HorarioApertura = horario
                ?? throw new ArgumentNullException(nameof(horario));

            DiasApertura = diasLaborales
                ?? throw new ArgumentNullException(nameof(diasLaborales));

            _diasCierre = diasCierre?.ToList()
                ?? throw new ArgumentNullException(nameof(diasCierre));
        }

        public void ActualizarHorario(Horario nuevoHorario)
        {
            HorarioApertura = nuevoHorario
                ?? throw new ArgumentNullException(nameof(nuevoHorario));
        }

        public void ActualizarDiasLaborales(DiasLaboralesMuseo nuevosDiasLaborales)
        {
            DiasApertura = nuevosDiasLaborales
                ?? throw new ArgumentNullException(nameof(nuevosDiasLaborales));
        }

        public void AgregarDiaCierre(DiaCierreMuseo diaCierre)
        {
            if (diaCierre == null)
                throw new ArgumentNullException(nameof(diaCierre));

            if (_diasCierre.Any(d => d.Id == diaCierre.Id && d.Id != ""))
                throw new DomainException("El día de cierre ya existe en el calendario.");

            _diasCierre.Add(diaCierre);
        }

        public void RemoverDiaCierre(DiaCierreMuseo diaCierre)
        {
            if (diaCierre == null)
                throw new ArgumentNullException(nameof(diaCierre));

            _diasCierre.Remove(diaCierre);
        }

        public bool EsDiaOperativo(DateTime fecha)
        {
            if (!DiasApertura.EsDiaLaboral(fecha.DayOfWeek))
                return false;

            var inicioDia = fecha.Date;
            var finDia = fecha.Date.AddDays(1);

            return !_diasCierre.Any(d =>
                d.SolapaConFechas(inicioDia, finDia));
        }

        public bool EstaAbierto(DateTime inicio, DateTime fin)
        {
            if (inicio >= fin)
                throw new ArgumentException("La fecha de inicio debe ser anterior a la fecha de fin.");

            if (!DiasApertura.EsDiaLaboral(inicio.DayOfWeek))
                return false;

            if (inicio.Date != fin.Date && !DiasApertura.EsDiaLaboral(fin.DayOfWeek))
                return false;

            if (_diasCierre.Any(d => d.SolapaConFechas(inicio, fin)))
                return false;

            return inicio.TimeOfDay >= HorarioApertura.HoraInicio.ToTimeSpan()
                && fin.TimeOfDay <= HorarioApertura.HoraFin.ToTimeSpan();
        }

        public IEnumerable<TimeSlot> ObtenerFranjasOperativas(DateOnly fecha)
        {
            var fechaDateTime = fecha.ToDateTime(TimeOnly.MinValue);

            if (!EsDiaOperativo(fechaDateTime))
                return [];

            var horaInicio = fecha.ToDateTime(HorarioApertura.HoraInicio);
            var horaFin = fecha.ToDateTime(HorarioApertura.HoraFin);

            var cierresDia = _diasCierre
                .Where(d => d.SolapaConFechas(horaInicio, horaFin))
                .OrderBy(d => d.FechaDesde)
                .ToList();

            if (!cierresDia.Any())
            {
                return [new TimeSlot(horaInicio, horaFin)];
            }

            var franjas = new List<TimeSlot>();
            var inicioFranja = horaInicio;

            foreach (var cierre in cierresDia)
            {
                var inicioCierre = cierre.FechaDesde > horaInicio ? cierre.FechaDesde : horaInicio;
                var finCierre = cierre.FechaHasta < horaFin ? cierre.FechaHasta : horaFin;

                if (inicioFranja < inicioCierre)
                {
                    franjas.Add(new TimeSlot(inicioFranja, inicioCierre));
                }

                inicioFranja = finCierre;
            }

            if (inicioFranja < horaFin)
            {
                franjas.Add(new TimeSlot(inicioFranja, horaFin));
            }

            return franjas;
        }
    }
}
