using Application.VisitaGrupal.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.VisitasGrupales.Entities.GrupalAutoguiada;

namespace Infrastructure.Repositories.Sql.VisitaGrupal
{
    internal sealed class RepositorioVisitaAutoguiadaReprogramada(MuseoDbContext context) : BaseRepository<VisitaAutoguiadaReprogramada>(context), IRepositorioAutoguiadaReprogramada
    {
    }
}
