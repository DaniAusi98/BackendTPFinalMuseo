using Application.VisitaGrupal.DataTransferObjets;

namespace Application.VisitaGrupal.ApplicationServices
{
    public interface IConsultaBloqueosDisponibilidad
    {
        Task<List<BloqueoDto>> ObtenerBloqueos(DateTime desde, DateTime hasta);

    }
}

