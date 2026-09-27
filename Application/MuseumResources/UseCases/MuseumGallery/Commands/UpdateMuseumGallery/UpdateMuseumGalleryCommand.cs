using System.ComponentModel.DataAnnotations;

using Core.Application;

using static Domain.RecursoMuseo.Enums.Enums;

namespace Application.MuseumResources.UseCases.MuseumGallery.Commands.UpdateMuseumGallery
{
    public class UpdateMuseumGalleryCommand:IRequestCommand
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string CodigoSala { get; set; }
        [Required]
        public EstadoSala EstadoSala  { get; set; }
        [Required]
        public TipoSala TipoSala { get; set; }
        [Required]
        public int Capacidad { get; set; }
        [Required]
        public UbicacionSala UbicacionSala { get; set; }
        public UpdateMuseumGalleryCommand()
        {
                
        }
    }
}
