using Core.Domain.Entities;
using Domain.Common.Exceptions;
using Domain.Common.ValueObjets;

namespace Domain.Common.Entities
{
    public class HorarioAperturaMuseo : DomainEntity<string>
    {
        public List<HorarioApertura> HorariosApertura { get; private set; }

        private readonly List<DiaCierreMuseo> _diasCierre;
        public IReadOnlyCollection<DiaCierreMuseo> DiasCierre
            => _diasCierre.AsReadOnly();
        protected HorarioAperturaMuseo()
        {
            _diasCierre = new List<DiaCierreMuseo>();
        }

        public HorarioAperturaMuseo(
            IEnumerable<HorarioApertura> horarios,
            IReadOnlyCollection<DiaCierreMuseo> diasCierre)
        {
            Id = Guid.NewGuid().ToString();

            HorariosApertura = horarios.ToList() ?? throw new ArgumentNullException(nameof(horarios));



            _diasCierre = diasCierre?.ToList()
                ?? throw new ArgumentNullException(nameof(diasCierre));
        }

        public void ActualizarHorario(IEnumerable<HorarioApertura> nuevosHorario)
        {
            HorariosApertura = nuevosHorario.ToList() ?? throw new ArgumentNullException(nameof(nuevosHorario));
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
        public bool EsDiaApertura(DateTime fecha)
        {
            DayOfWeek diaSemana = fecha.DayOfWeek;

            TimeOnly hora = TimeOnly.FromDateTime(fecha);

            bool esDiaApertura = HorariosApertura.Any(a =>
                a.Dia == diaSemana &&
                hora >= a.HoraInicio &&
                hora <= a.HoraFin
            );



            return esDiaApertura;
        }

        public bool EsDiaOperativo(DateTime fecha)
        {
            if (!EsDiaApertura(fecha))
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

            // Validar tu regla de negocio: inicio y fin deben ser del mismo día
            if (inicio.Date != fin.Date)
                return false; // O podrías lanzar una excepción según prefieras

            // Validar si el día/rango se solapa con días de cierre especiales (vacaciones, feriados, etc.)
            if (_diasCierre.Any(d => d.SolapaConFechas(inicio, fin)))
                return false;

            // Extraemos las variables que necesitamos para comparar con la lista
            DayOfWeek diaSemana = inicio.DayOfWeek;
            TimeOnly horaInicio = TimeOnly.FromDateTime(inicio);
            TimeOnly horaFin = TimeOnly.FromDateTime(fin);

            // Buscamos si existe AL MENOS UN horario en la lista que cubra COMPLETAMENTE el intervalo
            bool horarioAbierto = HorariosApertura.Any(objeto =>
                objeto.Dia == diaSemana &&
                horaInicio >= objeto.HoraInicio &&
                horaFin <= objeto.HoraFin
            );

            return horarioAbierto;
        }


        public IEnumerable<TimeSlot> ObtenerFranjasOperativas(DateOnly fecha)
        {
            DayOfWeek diaSemana = fecha.DayOfWeek;

            // 1. Buscamos todas las franjas teóricas de apertura programadas para este día de la semana
            var horariosDelDia = HorariosApertura
                .Where(a => a.Dia == diaSemana)
                .ToList();

            // Si el museo de por sí no abre este día de la semana, devolvemos vacío inmediatamente
            if (!horariosDelDia.Any())
                return Enumerable.Empty<TimeSlot>();

            var franjasDisponibles = new List<TimeSlot>();

            // 2. Procesamos CADA turno que tenga el museo configurado para este día
            foreach (var horario in horariosDelDia)
            {
                // Combinamos la fecha recibida con las horas "TimeOnly" de este horario operativo
                var horaInicio = fecha.ToDateTime(horario.HoraInicio);
                var horaFin = fecha.ToDateTime(horario.HoraFin);

                // 3. Buscamos los cierres (DateTime Desde/Hasta) que se solapen con esta franja horaria específica
                var cierresFranja = _diasCierre
                    .Where(d => d.SolapaConFechas(horaInicio, horaFin))
                    .OrderBy(d => d.FechaDesde)
                    .ToList();

                // Si no hay ningún cierre interfiriendo con este turno, la franja operativa se agrega completa
                if (!cierresFranja.Any())
                {
                    franjasDisponibles.Add(new TimeSlot(horaInicio, horaFin));
                    continue;
                }

                // 4. Algoritmo de sustracción: Recortamos los espacios bloqueados por los cierres
                var inicioFranja = horaInicio;

                foreach (var cierre in cierresFranja)
                {
                    // Acotamos los límites del cierre al espacio de nuestra franja por si el cierre es más largo (ej: dura varios días)
                    var inicioCierre = cierre.FechaDesde > horaInicio ? cierre.FechaDesde : horaInicio;
                    var finCierre = cierre.FechaHasta < horaFin ? cierre.FechaHasta : horaFin;

                    // Si hay un espacio disponible antes de que empiece el cierre, creamos esa franja libre
                    if (inicioFranja < inicioCierre)
                    {
                        franjasDisponibles.Add(new TimeSlot(inicioFranja, inicioCierre));
                    }

                    // El próximo inicio disponible será cuando termine este cierre
                    inicioFranja = finCierre;
                }

                // Si después del último cierre todavía queda tiempo disponible en el turno, agregamos el remanente
                if (inicioFranja < horaFin)
                {
                    franjasDisponibles.Add(new TimeSlot(inicioFranja, horaFin));
                }
            }

            return franjasDisponibles;
        }

    }
}
