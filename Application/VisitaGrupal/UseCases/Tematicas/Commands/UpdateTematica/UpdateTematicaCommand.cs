using System.ComponentModel.DataAnnotations;

using Core.Application;

namespace Application.VisitaGrupal.UseCases.Tematicas.Commands.UpdateTematica
{
    public class UpdateTematicaCommand : IRequestCommand
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Disponible { get; set; }
        [Required]
        public List<int> SalaIds { get; set; } = new();
    }
}
