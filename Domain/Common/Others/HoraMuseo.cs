using TimeZoneConverter;

namespace Domain.Common.Others
{
    public static class HoraMuseo
    {
        private static readonly TimeZoneInfo ZonaHoraria =
            TZConvert.GetTimeZoneInfo("America/Argentina/Cordoba");

        public static DateTime Ahora()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.UtcNow,
                ZonaHoraria);
        }

        public static DateTime Hoy()
        {
            return Ahora().Date;
        }
    }
}