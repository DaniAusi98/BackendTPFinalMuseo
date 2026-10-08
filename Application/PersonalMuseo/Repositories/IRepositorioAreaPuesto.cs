using Core.Application.Repositories;
using Domain.PersonalMuseo.Entities.UsuarioInterno;

namespace Application.PersonalMuseo.Repositories
{
    public interface IRepositorioAreaPuesto : IRepository<AreaPuesto>
    {
        public Task<List<Puesto>> ObtenerPuestosPorArea(string areaId);

    }
}
