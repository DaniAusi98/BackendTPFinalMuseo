using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Others
{
    public static class HoraMuseo
    {
        public static DateTime Hoy()
        {
            var zona = TimeZoneInfo.FindSystemTimeZoneById(
                "America/Argentina/Cordoba");

            return TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.UtcNow,
                zona).Date;
        }
    }
}
