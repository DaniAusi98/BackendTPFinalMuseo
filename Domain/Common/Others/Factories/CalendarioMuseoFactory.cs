using Domain.Common.Entities;
using Domain.Common.ValueObjets;

namespace Domain.Common.Others.Factories
{
    public static class CalendarioMuseoFactory
    {
        public static HorarioAperturaMuseo CrearCalendarioPorDefecto()
        {
            TimeOnly desde = new(9, 0);
            TimeOnly hasta = new(20, 0);

            // C# infiere el tipo automáticamente usando la inicialización de colecciones
            List<HorarioApertura> horariosApertura =
            [
                new(DayOfWeek.Monday, desde, hasta),
                new(DayOfWeek.Tuesday, desde, hasta),
                new(DayOfWeek.Wednesday, desde, hasta),
                new(DayOfWeek.Thursday, desde, hasta),
                new(DayOfWeek.Friday, desde, hasta)
            ];

            var diasCierre = new List<DiaCierreMuseo>();

            return new HorarioAperturaMuseo(
                horariosApertura,
                diasCierre
            );
        }
    }
}
