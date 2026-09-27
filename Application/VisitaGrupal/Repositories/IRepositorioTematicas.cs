using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Application.Repositories;

using Domain.VisitasGrupales.Entities;

namespace Application.VisitaGrupal.Repositories
{
    public interface IRepositorioTematicas:IRepository<TematicaVisita>
    {
        Task<List<TematicaVisita>> ObtenerDisponiblesAsync();
        Task<List<TematicaVisita>> GetByIdsAsync(List<string> tematicasIds);

    }
}
