
using Core.Domain.Entities;
using Domain.Common.Entities;
using Domain.Common.Exceptions;
using Domain.VisitasGrupales.ValueObjects;

namespace Domain.VisitasGrupales.Entities.GrupalGuiada
{
    public class ConfiguracionVisitasGrupalesGuiadas : DomainEntity<string>
    {
        private readonly List<BloqueoVisitasGuiadas> _bloqueos;

        public List<HorarioDisponible> DiasDisponibles { get; private set; }

        public int CapacidadPorGuia { get; private set; }

        public int CapacidadMaximaPorTurno { get; private set; }

        public int MaximoVisitasSimultaneas { get; private set; }

        public IReadOnlyCollection<BloqueoVisitasGuiadas> Bloqueos
            => _bloqueos.AsReadOnly();

        protected ConfiguracionVisitasGrupalesGuiadas()
        {
            _bloqueos = new List<BloqueoVisitasGuiadas>();
            DiasDisponibles = new List<HorarioDisponible>();
        }

        public ConfiguracionVisitasGrupalesGuiadas(
            int capacidadPorGuia,
            int capacidadMaxima,
            int maximoVisitasSimultaneas,
            IEnumerable<HorarioDisponible> diasDisponibles,
            HorarioAperturaMuseo calendario,
            IReadOnlyCollection<BloqueoVisitasGuiadas>? bloqueos = null)
        {
            Id = Guid.NewGuid().ToString();

            if (calendario == null)
                throw new ArgumentNullException(nameof(calendario));

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

            if (diasDisponibles == null)
                throw new ArgumentNullException(nameof(diasDisponibles));

            var listaDias = diasDisponibles.ToList();

            if (!listaDias.Any())
                throw new DomainException(
                    "Debe existir al menos un horario disponible.");

            ValidarHorariosDentroDelCalendario(listaDias, calendario);
            ValidarHorariosNoSeSolapen(listaDias);

            CapacidadPorGuia = capacidadPorGuia;
            CapacidadMaximaPorTurno = capacidadMaxima;
            MaximoVisitasSimultaneas = maximoVisitasSimultaneas;
            DiasDisponibles = listaDias;

            _bloqueos = bloqueos?.ToList()
                ?? new List<BloqueoVisitasGuiadas>();
        }

        public void ActualizarDiasDisponibles(
            IEnumerable<HorarioDisponible> nuevosDias,
            HorarioAperturaMuseo calendario)
        {
            if (nuevosDias == null)
                throw new ArgumentNullException(nameof(nuevosDias));

            if (calendario == null)
                throw new ArgumentNullException(nameof(calendario));

            var listaNuevosDias = nuevosDias.ToList();

            if (!listaNuevosDias.Any())
                throw new DomainException(
                    "Debe existir al menos un horario disponible.");

            ValidarHorariosDentroDelCalendario(
                listaNuevosDias,
                calendario);

            ValidarHorariosNoSeSolapen(listaNuevosDias);

            DiasDisponibles = listaNuevosDias;
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
            bool tieneHorarioEseDia = DiasDisponibles.Any(
                d => d.Dia == fecha.DayOfWeek);

            if (!tieneHorarioEseDia)
                return false;

            return !_bloqueos.Any(b => b.SolapaConFecha(fecha));
        }

        public int CalcularCapacidadDisponible(int guiasDisponibles)
        {
            if (guiasDisponibles <= 0)
                return 0;

            return Math.Min(
                guiasDisponibles * CapacidadPorGuia,
                CapacidadMaximaPorTurno);
        }

        private void ValidarHorariosNoSeSolapen(
            List<HorarioDisponible> horarios)
        {
            var horariosPorDia = horarios.GroupBy(h => h.Dia);

            foreach (var grupoDia in horariosPorDia)
            {
                var lista = grupoDia.ToList();

                for (int i = 0; i < lista.Count; i++)
                {
                    for (int j = i + 1; j < lista.Count; j++)
                    {
                        bool seSolapan =
                            lista[i].Desde < lista[j].Hasta &&
                            lista[j].Desde < lista[i].Hasta;

                        if (seSolapan)
                        {
                            throw new DomainException(
                                $"Conflicto de horarios para el día {grupoDia.Key}: " +
                                $"{lista[i].Desde}-{lista[i].Hasta} se solapa con " +
                                $"{lista[j].Desde}-{lista[j].Hasta}.");
                        }
                    }
                }
            }
        }

        private void ValidarHorariosDentroDelCalendario(
            IEnumerable<HorarioDisponible> horariosAValidar,
            HorarioAperturaMuseo calendario)
        {
            foreach (var horarioVisita in horariosAValidar)
            {
                var horarioMuseo = calendario.HorariosApertura
                    .FirstOrDefault(h => h.Dia == horarioVisita.Dia);

                if (horarioMuseo == null)
                {
                    throw new DomainException(
                        $"El museo no abre los días {horarioVisita.Dia}. " +
                        "No se pueden configurar visitas guiadas.");
                }

                if (horarioVisita.Desde < horarioMuseo.HoraInicio ||
                    horarioVisita.Hasta > horarioMuseo.HoraFin)
                {
                    throw new DomainException(
                        $"El horario configurado para el día {horarioVisita.Dia} " +
                        $"({horarioVisita.Desde} - {horarioVisita.Hasta}) excede " +
                        $"el horario de apertura del museo " +
                        $"({horarioMuseo.HoraInicio} - {horarioMuseo.HoraFin}).");
                }
            }
        }
    }
}
