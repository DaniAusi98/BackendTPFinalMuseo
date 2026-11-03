using Application.Eventos.DataTransferObjets;
using Core.Application;
using System.ComponentModel.DataAnnotations;

namespace Application.Eventos.UseCases.Queries.GetPropuestaByQuery
{
    public class GetPropuestaByIdQuery : IRequestQuery<PropuestaEventoDto>
    {
        [Required]
        public string PropuestaId { get; set; }

        public GetPropuestaByIdQuery()
        {
        }
    }
}
