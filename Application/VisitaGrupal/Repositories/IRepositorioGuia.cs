using Core.Application.Repositories;

using Domain.RecursoMuseo.Entities.Guia;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.VisitaGrupal.Repositories
{
    public interface IRepositorioGuia : IRepository<Guia>
    {
        Task<List<Guia>> ObtenerGuiasConDisponibilidadAsync();
    }
}
