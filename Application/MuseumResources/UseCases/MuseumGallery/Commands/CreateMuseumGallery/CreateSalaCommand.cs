
using Core.Application;
using System.ComponentModel.DataAnnotations;
using static Domain.RecursoMuseo.Enums.Enums;

namespace Application.MuseumResources.UseCases.MuseumGallery.Commands.CreateMuseumGallery
{
    public class CreateSalaCommand : IRequestCommand<string>
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

        public CreateSalaCommand()
        {

        }
    }
}
