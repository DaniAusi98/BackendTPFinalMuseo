using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.Repositories;

using Core.Infraestructure.Repositories.Sql;
using Domain.Common.Entities;

namespace Infrastructure.Repositories.Sql.DisponibilidadActividades
{
    internal sealed class RepositorioDiaCierreMuseo(MuseoDbContext context) : BaseRepository<DiaCierreMuseo>(context), IRepositorioDiaCierreMuseo
    {
    }
}
