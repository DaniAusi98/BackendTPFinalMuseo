using Application.ApplicationMuseo.DataTransferObjects;
using Core.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ApplicationMuseo.UseCases.Provincia.Queries.GetAllProvincias
{
    public class GetAllProvinciasQuery: QueryRequest<QueryResult<ProvinciaDto>>
    {
    }
}
