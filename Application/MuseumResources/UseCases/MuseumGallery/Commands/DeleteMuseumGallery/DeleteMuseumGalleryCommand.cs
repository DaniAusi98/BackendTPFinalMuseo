using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Application;

using MediatR;

namespace Application.MuseumResources.UseCases.MuseumGallery.Commands.DeleteMuseumGallery
{
    public class DeleteMuseumGalleryCommand:IRequestCommand<Unit>
    {
        [Required]
        public int MuseumGalleryId { get; set; }
        public DeleteMuseumGalleryCommand()
        {
        }
    }
}
/* public class DeleteDummyEntityCommand : IRequestCommand<Unit>
    {
        [Required]
        public int DummyIdProperty { get; set; }

        public DeleteDummyEntityCommand()
        {
        }
    }*/
