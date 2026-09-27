using Core.Application;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.ApplicationMuseo.UseCases.DummyEntity.Commands.DeleteDummyEntity
{
    public class DeleteDummyEntityCommand : IRequestCommand<Unit>
    {
        [Required]
        public int DummyIdProperty { get; set; }

        public DeleteDummyEntityCommand()
        {
        }
    }
}
