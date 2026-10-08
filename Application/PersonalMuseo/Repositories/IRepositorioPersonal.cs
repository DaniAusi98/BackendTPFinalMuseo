using Core.Application.Repositories;
using Domain.PersonalMuseo.Entities.UsuarioInterno;

namespace Application.PersonalMuseo.Repositories
{
    public interface IRepositorioPersonal : IRepository<Personal>
    {
        Task<Personal?> GetPersonalMuseo(string userId);



    }
}
