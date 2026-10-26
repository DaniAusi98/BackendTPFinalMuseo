using Core.Application;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.ConfiguracionSistema.UseCases.Commands.DeleteClosyngDay
{
    public class DeleteClosyngDayCommand : IRequestCommand<Unit>
    {
        [Required]
        public string Id { get; set; }
        public DeleteClosyngDayCommand()
        {

        }
    }
}
