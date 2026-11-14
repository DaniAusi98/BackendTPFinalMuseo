using Core.Application;

namespace Application.Eventos.UseCases.Commands.CancelarMuestra
{
    public class CancelarMuestraCommand : IRequestCommand
    {
        public string MuestraId { get; set; }

        public CancelarMuestraCommand()
        {

        }
    }
}
