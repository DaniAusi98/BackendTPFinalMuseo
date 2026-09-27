using Core.Application;

using MediatR;

using System.ComponentModel.DataAnnotations;

using static Domain.Common.Enums.Enums;

namespace Application.ApplicationMuseo.UseCases.DummyEntity.Commands.UpdateDummyEntity
{
    public class UpdateDummyEntityCommand : IRequestCommand
    {
        [Required]
        public int DummyIdProperty { get; set; }
        [Required]
        public string dummyPropertyOne { get; set; }
        public DummyValues dummyPropertyTwo { get; set; }

        public UpdateDummyEntityCommand()
        {
        }
    }
}
