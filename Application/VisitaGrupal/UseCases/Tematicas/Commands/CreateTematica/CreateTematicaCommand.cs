using System.ComponentModel.DataAnnotations;

using Core.Application;

namespace Application.VisitaGrupal.UseCases.Tematicas.Commands.CreateTematica
{
    public class CreateTematicaCommand : IRequestCommand<string>
    {
        [Required]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        [Required]
        public List<int> SalaIds { get; set; } = new();
    }
}
