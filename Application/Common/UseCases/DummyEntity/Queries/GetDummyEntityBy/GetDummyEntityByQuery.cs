using Application.ApplicationMuseo.DataTransferObjects;
using Core.Application;
using System.ComponentModel.DataAnnotations;

namespace Application.ApplicationMuseo.UseCases.DummyEntity.Queries.GetDummyEntityBy
{
    public class GetDummyEntityByQuery : IRequestQuery<DummyEntityDto>
    {
        [Required]
        public string DummyIdProperty { get; set; }

        public GetDummyEntityByQuery()
        {
        }
    }
}
