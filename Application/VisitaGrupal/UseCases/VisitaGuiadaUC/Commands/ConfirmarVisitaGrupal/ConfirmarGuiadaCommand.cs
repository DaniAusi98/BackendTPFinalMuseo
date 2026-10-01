using Core.Application;


namespace Application.VisitaGrupal.UseCases.VisitaGuiadaUC.Commands.ConfirmarVisitaGrupal
{
    public class ConfirmarGuiadaCommand:IRequestCommand
    {
        public string ReservationId { get; set; }

        public ConfirmarGuiadaCommand()
        {
                
        }
    }
}
