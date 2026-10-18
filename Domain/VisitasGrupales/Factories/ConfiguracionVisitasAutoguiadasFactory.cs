using Domain.Common.Entities;
using Domain.VisitasGrupales.Entities.GrupalAutoguiada;
using Domain.VisitasGrupales.ValueObjects;

namespace Domain.VisitasGrupales.Factories
{
    public static class ConfiguracionVisitasAutoguiadasFactory
    {
        /// <summary>
        /// Crea una configuración por defecto para visitas autoguiadas (Lunes a Viernes de 9:00 a 18:00).
        /// </summary>
        public static ConfiguracionHorarioAutoguiada CrearConfiguracionPorDefecto(HorarioAperturaMuseo calendario)
        {
            if (calendario == null)
                throw new ArgumentNullException(nameof(calendario));

            TimeOnly desde = new(9, 0);
            TimeOnly hasta = new(18, 0);

            // CORREGIDO: Mapeamos los días de Lunes a Viernes usando el nuevo HorarioDisponible
            var diasDisponibles = new List<HorarioDisponible>
            {
                new(DayOfWeek.Monday, desde, hasta),
                new(DayOfWeek.Tuesday, desde, hasta),
                new(DayOfWeek.Wednesday, desde, hasta),
                new(DayOfWeek.Thursday, desde, hasta),
                new(DayOfWeek.Friday, desde, hasta)
            };

            var duracionVisita = TimeSpan.FromHours(1);
            var intervaloReservas = TimeSpan.FromMinutes(30);

            // CORREGIDO: Firma adaptada a la entidad refactorizada
            return new ConfiguracionHorarioAutoguiada(
                capacidadMaximaPorGrupo: 25,
                diasDisponibles: diasDisponibles,
                calendario: calendario,
                duracionVisita: duracionVisita,
                intervaloReservas: intervaloReservas,
                visitasSimultaneasMaximas: 2,
                bloqueos: null
            );
        }

        /// <summary>
        /// Crea una configuración personalizada para visitas autoguiadas utilizando el nuevo modelo de listas.
        /// </summary>
        public static ConfiguracionHorarioAutoguiada CrearConfiguracionPersonalizada(
            int capacidadMaximaPorGrupo,
            IEnumerable<HorarioDisponible> diasDisponibles,
            HorarioAperturaMuseo calendario,
            TimeSpan duracionVisita,
            TimeSpan intervaloReservas,
            int visitasSimultaneasMaximas)
        {
            if (calendario == null)
                throw new ArgumentNullException(nameof(calendario));

            return new ConfiguracionHorarioAutoguiada(
                capacidadMaximaPorGrupo: capacidadMaximaPorGrupo,
                diasDisponibles: diasDisponibles,
                calendario: calendario,
                duracionVisita: duracionVisita,
                intervaloReservas: intervaloReservas,
                visitasSimultaneasMaximas: visitasSimultaneasMaximas,
                bloqueos: null
            );
        }

        /// <summary>
        /// Crea una configuración con slots sin solapamiento (Intervalo = Duración).
        /// Toma los horarios de apertura del museo como base para rellenar los días disponibles de forma automática.
        /// </summary>
        public static ConfiguracionHorarioAutoguiada CrearConfiguracionSinSolapamiento(
            HorarioAperturaMuseo calendario,
            TimeSpan duracionVisita,
            int capacidadMaximaPorGrupo = 25)
        {
            if (calendario == null)
                throw new ArgumentNullException(nameof(calendario));

            // CORREGIDO: Mapeamos dinámicamente basándonos en los días en que el museo sí opera
            var diasDisponibles = calendario.HorariosApertura
                .Select(h => new HorarioDisponible(h.Dia, h.HoraInicio, h.HoraFin))
                .ToList();

            return new ConfiguracionHorarioAutoguiada(
                capacidadMaximaPorGrupo: capacidadMaximaPorGrupo,
                diasDisponibles: diasDisponibles,
                calendario: calendario,
                duracionVisita: duracionVisita,
                intervaloReservas: duracionVisita, // Mismo valor = sin solapamiento
                visitasSimultaneasMaximas: 1,      // Solo 1 grupo a la vez
                bloqueos: null
            );
        }
    }
}
