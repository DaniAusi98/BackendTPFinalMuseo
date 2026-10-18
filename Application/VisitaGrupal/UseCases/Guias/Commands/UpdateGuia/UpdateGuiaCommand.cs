using Application.VisitaGrupal.DataTransferObjets.Guias;
using Core.Application;
using System.ComponentModel.DataAnnotations;

namespace Application.VisitaGrupal.UseCases.Guias.Commands.UpdateGuia
{
    public class UpdateGuiaCommand : IRequestCommand
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellido { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Telefono { get; set; }

        [Required]
        public List<GuiaHorarioDto> Horarios { get; set; } = new();

        public bool Activo { get; set; } = true;
    }
}