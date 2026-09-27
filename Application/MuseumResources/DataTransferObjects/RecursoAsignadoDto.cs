
using System.ComponentModel.DataAnnotations;


namespace Application.MuseumResources.DataTransferObjects
{
    
        public class RecursoAsignadoDto
        {
            [Required]
            public string RecursoId { get; set; } = string.Empty;

            [Required]
            public int CantidadAsignada { get; set; }
        }

    
}
