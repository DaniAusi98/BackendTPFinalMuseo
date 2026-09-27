using Application.ActividadesEducativas.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.ActividadesAreaEducacion.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Sql.ActividadEducativa
{
    internal sealed class RepositorioActividadEducativa(MuseoDbContext context) : BaseRepository<Domain.ActividadesAreaEducacion.Entities.ActividadEducativa>(context), IRepositorioActividadEducativa
    {
    }
}
