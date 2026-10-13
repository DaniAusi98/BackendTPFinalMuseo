using Application.VisitaGrupal.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.VisitasGrupales.Entities.GrupalGuiada;

namespace Infrastructure.Repositories.Sql.VisitaGrupal
{
    internal sealed class RepositorioVisitaReprogramada(MuseoDbContext context) : BaseRepository<VisitaReprogramada>(context), IReporsitorioVisitaReprogramada
    {
    }
}
