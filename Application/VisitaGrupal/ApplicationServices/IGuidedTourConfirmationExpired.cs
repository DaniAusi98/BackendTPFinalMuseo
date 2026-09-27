
namespace Application.VisitaGrupal.ApplicationServices
{
    public interface IGuidedTourConfirmationExpired
    {
       Task CancelGuidedTourAsync(string reservaId,DateTime fechaLimite);
    }
}
