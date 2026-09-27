
using Application.ApplicationMuseo.ApplicationServices;

namespace Infrastructure.Adapters
{
    public sealed class ArgentinaClock : IClock
    {
        private readonly TimeZoneInfo _timeZone;

        public ArgentinaClock()
        {
            _timeZone = OperatingSystem.IsWindows()
                ? TimeZoneInfo.FindSystemTimeZoneById("Argentina Standard Time")
                : TimeZoneInfo.FindSystemTimeZoneById("America/Argentina/Cordoba");
        }

        public DateTime Now()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.UtcNow,
                _timeZone);
        }
    }
}
