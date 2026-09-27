using Domain.Common.Entities;

namespace Application.Common.ApplicationServices
{
    public interface IConsultarProvinciasArgetina
    {
        Task<List<Provincia>> ObtenerProvinciasArgentinasAsync();
    }
}
