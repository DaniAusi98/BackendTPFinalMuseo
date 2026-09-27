using Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.ApplicationServices
{
    public interface IConsultaDepartamentos
    {
        Task<List<Departamento>> ObtenerDepartamentosArgentinaAsync();

    }
}
