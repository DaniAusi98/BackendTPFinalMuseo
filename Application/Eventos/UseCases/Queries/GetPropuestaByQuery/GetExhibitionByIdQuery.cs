using Application.Eventos.DataTransferObjets;
using Core.Application;
using System.ComponentModel.DataAnnotations;

namespace Application.Eventos.UseCases.Queries.GetPropuestaByQuery
{
    public class GetExhibitionByIdQuery : IRequestQuery<PropuestaMuestraDto>
    {
        [Required]
        public string PropuestaId { get; set; }

        public GetExhibitionByIdQuery()
        {

        }
    }
}
