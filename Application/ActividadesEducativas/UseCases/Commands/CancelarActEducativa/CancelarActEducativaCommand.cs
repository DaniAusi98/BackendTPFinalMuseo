using Core.Application;

namespace Application.ActividadesEducativas.UseCases.Commands.CancelarActEducativa
{
    public class CancelarActEducativaCommand : IRequestCommand
    {
        public string ActEducativaId { get; set; }

        public CancelarActEducativaCommand()
        {

        }
    }
}
