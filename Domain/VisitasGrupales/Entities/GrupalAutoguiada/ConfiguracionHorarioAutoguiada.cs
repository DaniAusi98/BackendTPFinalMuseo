using Core.Domain.Entities;
using Domain.Common.Entities;
using Domain.Common.Exceptions;
using Domain.Common.ValueObjets;
using Domain.VisitasGrupales.ValueObjects;

namespace Domain.VisitasGrupales.Entities.GrupalAutoguiada
{
    public class ConfiguracionHorarioAutoguiada : DomainEntity<string>
    {
        private readonly List<BloqueoAutoguiada> _bloqueos;

        // Propiedades de la entidad alineadas con tu modelo de listas [1, 2, 3]
        public List<HorarioDisponible> DiasDisponibles { get; private set; }
        public int CapacidadMaximaPorGrupo { get; private set; }
        public TimeSpan DuracionVisita { get; private set; }
        public TimeSpan IntervaloReservas { get; private set; }
        public int VisitasSimultaneasMaximas { get; private set; }

        public IReadOnlyCollection<BloqueoAutoguiada> Bloqueos
            => _bloqueos.AsReadOnly();

        protected ConfiguracionHorarioAutoguiada()
        {
            _bloqueos = new List<BloqueoAutoguiada>();
            DiasDisponibles = new List<HorarioDisponible>();
        }

        public ConfiguracionHorarioAutoguiada(
            int capacidadMaximaPorGrupo,
            IEnumerable<HorarioDisponible> diasDisponibles,
            HorarioAperturaMuseo calendario,
            TimeSpan duracionVisita,
            TimeSpan intervaloReservas,
            int visitasSimultaneasMaximas,
            IReadOnlyCollection<BloqueoAutoguiada>? bloqueos = null)
        {
            Id = Guid.NewGuid().ToString();

            if (calendario == null)
                throw new ArgumentNullException(nameof(calendario));

            if (capacidadMaximaPorGrupo <= 0)
                throw new DomainException("La capacidad máxima por grupo debe ser mayor a 0.");

            if (duracionVisita <= TimeSpan.Zero)
                throw new DomainException("La duración de la visita debe ser mayor a 0.");

            if (intervaloReservas <= TimeSpan.Zero)
                throw new DomainException("El intervalo entre reservas debe ser mayor a 0.");

            if (intervaloReservas > duracionVisita)
                throw new DomainException("El intervalo entre reservas no puede ser mayor que la duración de la visita.");

            if (visitasSimultaneasMaximas <= 0)
                throw new DomainException("La cantidad de visitas simultáneas debe ser mayor a 0.");

            if (diasDisponibles == null)
                throw new ArgumentNullException(nameof(diasDisponibles));

            var listaDias = diasDisponibles.ToList();

            // Validación cruzada inicial contra las listas del museo
            ValidarHorariosDentroDelCalendario(listaDias, calendario);

            CapacidadMaximaPorGrupo = capacidadMaximaPorGrupo;
            DiasDisponibles = listaDias;
            DuracionVisita = duracionVisita;
            IntervaloReservas = intervaloReservas;
            VisitasSimultaneasMaximas = visitasSimultaneasMaximas;
            _bloqueos = bloqueos?.ToList() ?? new List<BloqueoAutoguiada>();
        }

        public void ActualizarDiasDisponibles(
            IEnumerable<HorarioDisponible> nuevosDias,
            HorarioAperturaMuseo calendario)
        {
            if (nuevosDias == null) throw new ArgumentNullException(nameof(nuevosDias));
            if (calendario == null) throw new ArgumentNullException(nameof(calendario));

            var listaNuevosDias = nuevosDias.ToList();
            ValidarHorariosDentroDelCalendario(listaNuevosDias, calendario);

            DiasDisponibles = listaNuevosDias;
        }

        public void ActualizarCapacidadMaxima(int capacidadMaximaPorGrupo)
        {
            if (capacidadMaximaPorGrupo <= 0)
                throw new DomainException("La capacidad máxima por grupo debe ser mayor a 0.");

            CapacidadMaximaPorGrupo = capacidadMaximaPorGrupo;
        }

        public void ActualizarDuracionVisita(TimeSpan duracionVisita)
        {
            if (duracionVisita <= TimeSpan.Zero)
                throw new DomainException("La duración de la visita debe ser mayor a 0.");

            if (IntervaloReservas > duracionVisita)
                throw new DomainException("La duración de la visita no puede ser menor que el intervalo entre reservas.");

            DuracionVisita = duracionVisita;
        }

        public void ActualizarIntervaloReservas(TimeSpan intervaloReservas)
        {
            if (intervaloReservas <= TimeSpan.Zero)
                throw new DomainException("El intervalo entre reservas debe ser mayor a 0.");

            if (intervaloReservas > DuracionVisita)
                throw new DomainException("El intervalo entre reservas no puede ser mayor que la duración de la visita.");

            IntervaloReservas = intervaloReservas;
        }

        public void ActualizarVisitasSimultaneas(int visitasSimultaneasMaximas)
        {
            if (visitasSimultaneasMaximas <= 0)
                throw new DomainException("La cantidad de visitas simultáneas debe ser mayor a 0.");

            VisitasSimultaneasMaximas = visitasSimultaneasMaximas;
        }

        public void AgregarBloqueo(BloqueoAutoguiada bloqueo)
        {
            if (bloqueo == null) throw new ArgumentNullException(nameof(bloqueo));

            if (_bloqueos.Any(b => b.SolapaConPeriodo(bloqueo.FechaDesde, bloqueo.FechaHasta)))
                throw new DomainException("El bloqueo se solapa con un bloqueo existente.");

            _bloqueos.Add(bloqueo);
        }

        public void RemoverBloqueo(BloqueoAutoguiada bloqueo)
        {
            if (bloqueo == null) throw new ArgumentNullException(nameof(bloqueo));
            _bloqueos.Remove(bloqueo);
        }

        public bool EstaDisponibleEnFecha(DateTime fecha)
        {
            // Verifica si el día de la semana está habilitado en las listas
            bool esDiaHabilitado = DiasDisponibles.Any(d => d.Dia == fecha.DayOfWeek);
            if (!esDiaHabilitado) return false;

            return !_bloqueos.Any(b => b.SolapaConFecha(fecha));
        }

        public int CalcularCapacidadMaximaSimultanea()
        {
            return CapacidadMaximaPorGrupo * VisitasSimultaneasMaximas;
        }

        public IEnumerable<TimeSlot> GenerarSlotsDisponibles(DateOnly fecha)
        {
            var fechaDateTime = fecha.ToDateTime(TimeOnly.MinValue);

            if (!EstaDisponibleEnFecha(fechaDateTime))
                return Enumerable.Empty<TimeSlot>();

            var slots = new List<TimeSlot>();
            var horariosDelDia = DiasDisponibles.Where(d => d.Dia == fecha.DayOfWeek).ToList();

            foreach (var horario in horariosDelDia)
            {
                var horaActual = fecha.ToDateTime(horario.Desde);
                var horaFin = fecha.ToDateTime(horario.Hasta);

                while (horaActual.Add(DuracionVisita) <= horaFin)
                {
                    var finVisita = horaActual.Add(DuracionVisita);

                    // Verifica que la franja generada no choque con ningún bloqueo activo
                    bool estaBloqueado = _bloqueos.Any(b => b.SolapaConPeriodo(horaActual, finVisita));

                    if (!estaBloqueado)
                    {
                        slots.Add(new TimeSlot(horaActual, finVisita));
                    }

                    horaActual = horaActual.Add(IntervaloReservas);
                }
            }

            return slots;
        }

        private void ValidarHorariosDentroDelCalendario(
            IEnumerable<HorarioDisponible> horariosAValidar,
            HorarioAperturaMuseo calendario)
        {
            foreach (var horarioVisita in horariosAValidar)
            {
                // Buscamos el turno del museo equivalente al mismo día de la semana
                var horarioMuseo = calendario.HorariosApertura
                    .FirstOrDefault(h => h.Dia == horarioVisita.Dia);

                if (horarioMuseo == null)
                {
                    throw new DomainException(
                        $"El museo no abre los días {horarioVisita.Dia}. No se puede configurar este horario de visitas.");
                }

                // Validación lógica de franjas horarias encapsuladas en las listas
                if (horarioVisita.Desde < horarioMuseo.HoraInicio || horarioVisita.Hasta > horarioMuseo.HoraFin)
                {
                    throw new DomainException(
                        $"El horario configurado para el día {horarioVisita.Dia} ({horarioVisita.Desde} - {horarioVisita.Hasta}) " +
                        $"excede el horario de apertura del museo ({horarioMuseo.HoraInicio} - {horarioMuseo.HoraFin}).");
                }
            }
        }
    }
}
