using System.ComponentModel.DataAnnotations;

using Application.MuseumResources.DataTransferObjects;
using Core.Application;

namespace Application.MuseumResources.UseCases.MuseumGallery.Queries.GetSalaBy
{
    public class GetMuseumGalleryByQuery:IRequestQuery<SalaDto>
    {
        [Required]
        public int SalaId { get; set; }
        public GetMuseumGalleryByQuery()
        {
        }
    }
}
