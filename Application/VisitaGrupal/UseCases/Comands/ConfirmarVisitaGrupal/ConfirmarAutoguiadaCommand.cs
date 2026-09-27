using Core.Application;

namespace Application.VisitaGrupal.UseCases.Comands.ConfirmarVisitaGrupal
{
    public class ConfirmarAutoguiadaCommand : IRequestCommand
    {
        public string ReservationId { get; set; }
        public ConfirmarAutoguiadaCommand()
        {

        }
    }
}
