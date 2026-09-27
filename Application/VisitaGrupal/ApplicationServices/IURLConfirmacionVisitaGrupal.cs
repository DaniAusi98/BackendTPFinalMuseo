using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.VisitaGrupal.ApplicationServices
{
    public interface IURLConfirmacionVisitaGrupal
    {
        string GetUrlConfirmacionVisitaGrupal(
            string visitaId);
    }
}
