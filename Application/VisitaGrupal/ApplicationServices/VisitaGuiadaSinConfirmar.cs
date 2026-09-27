using Application.VisitaGrupal.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.VisitasGrupales.Enums.Enums;

namespace Application.VisitaGrupal.ApplicationServices
{
    public class VisitaGuiadaSinConfirmar(IRepositorioVisitaGuiada repositorioVisitaGuiada) : IVisitaGuiadaSinConfirmacion
    {
        private readonly IRepositorioVisitaGuiada _repositorioVisitaGuiada = repositorioVisitaGuiada ?? throw new ArgumentNullException(nameof(repositorioVisitaGuiada));   
        public async Task CancelarVisitaGuiadaSinConfirmacion(string reservaId)
        {
            var visita = await _repositorioVisitaGuiada.FindOneAsync(reservaId);
            if (visita == null)
            {
                throw new Exception("No se encontró la visita guiada con el ID proporcionado.");
            }
            if (visita.EstadoConfirmacion==EstadoConfirmacionVisita.PendienteConfirmar)
            {
                visita.CancelarVisitaGuiada();
                _repositorioVisitaGuiada.Update(visita.Id,visita);
            }
            else
            {
                throw new Exception("La visita guiada ya ha sido confirmada y no puede ser cancelada.");
            }

        }
    }
}
