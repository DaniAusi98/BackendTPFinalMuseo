using Application.MuseumResources.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MuseumResources.ApplicationServices
{
    public interface IDisponibilidadRecursosService
    {
        Task<List<DisponibilidadRecursoDto>> ConsultarAsync(
            DateTime inicio,
            DateTime fin,
            string? rrule);
    }
}
