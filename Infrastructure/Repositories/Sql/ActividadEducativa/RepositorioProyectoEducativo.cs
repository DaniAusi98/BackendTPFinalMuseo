using Application.ActividadesEducativas.Repositories;
using Application.ActividadMuseo.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.ActividadesAreaEducacion.Entities;


namespace Infrastructure.Repositories.Sql.ActividadEducativa
{
    internal sealed class RepositorioProyectoEducativo(MuseoDbContext context) : BaseRepository<ProyectosAreaEducacion>(context), IRepositorioProyectoAreaEducacion
    {
    }
}
