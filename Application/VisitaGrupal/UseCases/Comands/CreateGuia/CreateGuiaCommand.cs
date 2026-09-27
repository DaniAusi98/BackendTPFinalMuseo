using System.ComponentModel.DataAnnotations;
using Core.Application;
using Application.VisitaGrupal.DataTransferObjets;

namespace Application.VisitaGrupal.UseCases.Comands.CreateGuia
{
    public class CreateGuiaCommand : IRequestCommand<string>
    {
        [Required]
        public string NombreCompleto { get; set; }
        [Required]
        public string PersonalInternoId { get; set; }
        [Required]
        public List<GuiaHorarioDto> Horarios { get; set; } = new();
    }
}
