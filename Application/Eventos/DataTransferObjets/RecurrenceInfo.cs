using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Eventos.DataTransferObjets
{
    public record RecurrenceInfo(
     DateTime? Until,
     int? Count)
    {
        public bool HasEnd =>
            Until.HasValue || Count.HasValue;
    }
}
