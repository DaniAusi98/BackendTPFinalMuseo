using Core.Application;

namespace Application.VisitaGrupal.UseCases.Commands.CancelarVisitaAutoguiada
{
    public class CancelarVisitaAutoGuiadaCommand : IRequestCommand
    {
        public string ReservationId { get; set; }

        public CancelarVisitaAutoGuiadaCommand()
        {

        }
    }
}
