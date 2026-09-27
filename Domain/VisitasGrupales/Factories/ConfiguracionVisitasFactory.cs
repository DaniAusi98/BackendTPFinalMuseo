using Domain.Common.Entities;
using Domain.Common.ValueObjets;
using Domain.VisitasGrupales.Entities.GrupalGuiada;
using Domain.VisitasGrupales.ValueObjects;

namespace Domain.VisitasGrupales.Factories
{
    public static class ConfiguracionVisitasFactory
    {
        /// <summary>
        /// Crea configuración por defecto validando contra el calendario del museo
        /// </summary>
        public static ConfiguracionVisitasGrupalesGuiadas CrearConfiguracionPorDefecto(CalendarioMuseo calendario)
        {
            if (calendario == null)
                throw new ArgumentNullException(nameof(calendario));

            // IMPORTANTE: Usar los días del calendario del museo, no hardcodear
            var diasDisponibles = new DiasLaboralesMuseo(calendario.DiasApertura.Dias);

            var turnos = new List<TurnoVisitaGuiada>
            {
                new TurnoVisitaGuiada(new TimeOnly(9, 30), new TimeOnly(10, 30)),
                new TurnoVisitaGuiada(new TimeOnly(11, 0), new TimeOnly(12, 0)),
                new TurnoVisitaGuiada(new TimeOnly(14, 30), new TimeOnly(15, 30)),
                new TurnoVisitaGuiada(new TimeOnly(16, 0), new TimeOnly(17, 0))
            };

            var configuracion = new ConfiguracionVisitasGrupalesGuiadas(
                capacidadPorGuia: 25,

                capacidadMaxima: 50,
                maximoVisitasSimultaneas: 1,
                diasDisponibles: diasDisponibles,
                turnos: turnos
            );

            // Validar explícitamente contra el calendario
            configuracion.ActualizarDiasDisponibles(diasDisponibles, calendario);

            return configuracion;
        }

        /// <summary>
        /// Crea configuración personalizada validando contra el calendario del museo
        /// </summary>
        public static ConfiguracionVisitasGrupalesGuiadas CrearConfiguracionPersonalizada(
            CalendarioMuseo calendario,
            int minGuias,
            int capacidadPorGuia,
            int capacidadMaxima,
            IEnumerable<DayOfWeek> diasLaborales,
            IEnumerable<(TimeOnly inicio, TimeOnly fin)> horariosTurnos)
        {
            if (calendario == null)
                throw new ArgumentNullException(nameof(calendario));

            var dias = new DiasLaboralesMuseo(diasLaborales);

            var turnos = horariosTurnos
                .Select(t => new TurnoVisitaGuiada(t.inicio, t.fin))
                .ToList();

            var configuracion = new ConfiguracionVisitasGrupalesGuiadas(
                minGuias,
                capacidadPorGuia,
                capacidadMaxima,
                dias,
                turnos
            );

            // Validar explícitamente contra el calendario
            configuracion.ActualizarDiasDisponibles(dias, calendario);

            return configuracion;
        }
    }
}
