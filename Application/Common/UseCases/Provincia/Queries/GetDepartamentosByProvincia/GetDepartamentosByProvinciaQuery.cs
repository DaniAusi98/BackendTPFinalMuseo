using Application.ApplicationMuseo.DataTransferObjects;
using Core.Application;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ApplicationMuseo.UseCases.Provincia.Queries.GetDepartamentosByProvincia
{
    public class GetDepartamentosByProvinciaQuery: QueryRequest<QueryResult<DepartamentoDto>>
    {
        [Required]
        public string ProvinciaId { get; set; }
        public GetDepartamentosByProvinciaQuery()
        {
        }
    }
}
