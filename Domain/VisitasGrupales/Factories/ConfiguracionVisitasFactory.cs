using Domain.Common.Entities;
using Domain.VisitasGrupales.Entities.GrupalGuiada;
using Domain.VisitasGrupales.ValueObjects;

namespace Domain.VisitasGrupales.Factories
{
    public static class ConfiguracionVisitasFactory
    {
        public static ConfiguracionVisitasGrupalesGuiadas CrearConfiguracionPorDefecto(
            HorarioAperturaMuseo calendario)
        {
            ArgumentNullException.ThrowIfNull(calendario);

            var diasLaborales = new[]
            {
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday,
                DayOfWeek.Friday
            };

            var turnos = new[]
            {
                (Desde: new TimeOnly(9, 30), Hasta: new TimeOnly(10, 30)),
                (Desde: new TimeOnly(11, 0), Hasta: new TimeOnly(12, 0)),
                (Desde: new TimeOnly(14, 0), Hasta: new TimeOnly(15, 0)),
                (Desde: new TimeOnly(15, 30), Hasta: new TimeOnly(16, 0))
            };

            var diasDisponibles = diasLaborales
                .SelectMany(dia => turnos.Select(turno =>
                    new HorarioDisponible(
                        dia,
                        turno.Desde,
                        turno.Hasta)))
                .ToList();

            return new ConfiguracionVisitasGrupalesGuiadas(
                capacidadPorGuia: 25,
                capacidadMaxima: 50,
                maximoVisitasSimultaneas: 1,
                diasDisponibles: diasDisponibles,
                calendario: calendario);
        }
    }
}