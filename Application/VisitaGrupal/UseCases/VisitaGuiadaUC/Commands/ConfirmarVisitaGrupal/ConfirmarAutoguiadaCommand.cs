using Core.Application;

namespace Application.VisitaGrupal.UseCases.VisitaGuiadaUC.Commands.ConfirmarVisitaGrupal
{
    public class ConfirmarAutoguiadaCommand : IRequestCommand
    {
        public string ReservationId { get; set; }
        public ConfirmarAutoguiadaCommand()
        {

        }
    }
}
