using Core.Application;

namespace Application.VisitaGrupal.UseCases.Commands.ConfirmarVisitaGrupal
{
    public class ConfirmarAutoguiadaCommand : IRequestCommand
    {
        public string ReservationId { get; set; }
        public ConfirmarAutoguiadaCommand()
        {

        }
    }
}
