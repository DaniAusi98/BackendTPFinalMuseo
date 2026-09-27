
using System.ComponentModel.DataAnnotations;

using Core.Application;

using static Domain.RecursoMuseo.Enums.Enums;

namespace Application.MuseumResources.UseCases.MuseumGallery.Commands.CreateMuseumGallery
{
    public class CreateMuseumGalleryCommand : IRequestCommand<string>
    {
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string CodigoSala { get; set; }
        [Required]
        public TipoSala TipoSala { get; set; }
        [Required]
        public int Capacidad { get; set; }
        [Required]
        public UbicacionSala Ubicacion { get; set; }

        public CreateMuseumGalleryCommand()
        {
                
        }
    }
}
