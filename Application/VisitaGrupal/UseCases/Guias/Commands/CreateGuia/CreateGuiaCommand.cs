using Application.VisitaGrupal.DataTransferObjets.Guias;
using Core.Application;
using System.ComponentModel.DataAnnotations;

namespace Application.VisitaGrupal.UseCases.Guias.Commands.CreateGuia
{
    public class CreateGuiaCommand : IRequestCommand<string>
    {
        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellido { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Telefono { get; set; }

        public List<GuiaHorarioDto> Horarios { get; set; } = new();
    }
}