using Core.Application;

namespace Application.VisitaGrupal.UseCases.Commands.CancelarVisitaGuiada
{
    public class CancelarVisitaGuiadaCommand : IRequestCommand
    {
        public string ReservationId { get; set; }

        public CancelarVisitaGuiadaCommand()
        {

        }
    }
}
