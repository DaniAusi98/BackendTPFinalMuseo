using Core.Application;
using System.ComponentModel.DataAnnotations;
using static Domain.Common.Enums.Enums;

namespace Application.ConfiguracionSistema.UseCases.Commands.CreateClosyngDay
{
    public class CreateClosyngDayCommand : IRequestCommand<string>
    {
        [Required]
        public DateTime Fecha { get; set; }

        public DateTime? FechaHasta { get; set; }

        [Required]
        public MotivoCierreMuseo Motivo { get; set; }

        [Required]
        public string Observaciones { get; set; }

        public CreateClosyngDayCommand()
        {

        }
    }
}
