namespace Application.VisitaGrupal.ApplicationServices
{
    internal interface ITurnoApplicationService
    {
        Task<bool> TurnosExistenAsync(DateOnly desde, DateOnly hasta);
    }
}
