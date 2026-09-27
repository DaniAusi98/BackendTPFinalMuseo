using System.ComponentModel.DataAnnotations;
using Core.Application;
using static Domain.RecursoMuseo.Enums.Enums;

namespace Application.MuseumResources.UseCases.Recurso.Commands.UpdateRecurso
{
    public class UpdateRecursoCommand : IRequestCommand
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string NombreRecurso { get; set; }
        [Required]
        public TipoRecurso TipoRecurso { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required]
        public EstadoRecurso Estado { get; set; }
        [Required]
        public int CantidadTotal { get; set; }

        public UpdateRecursoCommand()
        {
        }
    }
}
