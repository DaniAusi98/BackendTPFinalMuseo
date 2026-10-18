using Application.ConfiguracionSistema.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.Common.Entities;
using Infrastructure.Repositories.Sql;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Sql.ActividadMuseo
{
    internal sealed class RepositorioCalendarioMuseo : BaseRepository<HorarioAperturaMuseo>, IRepositorioCalendarioMuseo
    {
        public RepositorioCalendarioMuseo(MuseoDbContext context) : base(context)
        {
        }

        public async Task<HorarioAperturaMuseo?> ObtenerCalendarioActivoAsync()
        {
            var calendarios = await Repository.ToListAsync();
            return calendarios.FirstOrDefault();
        }
    }
}
