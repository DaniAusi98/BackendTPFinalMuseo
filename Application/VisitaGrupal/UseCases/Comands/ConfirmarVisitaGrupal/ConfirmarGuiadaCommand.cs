using Core.Application;


namespace Application.VisitaGrupal.UseCases.Comands.ConfirmarVisitaGrupal
{
    public class ConfirmarGuiadaCommand:IRequestCommand
    {
        public string ReservationId { get; set; }

        public ConfirmarGuiadaCommand()
        {
                
        }
    }
}
