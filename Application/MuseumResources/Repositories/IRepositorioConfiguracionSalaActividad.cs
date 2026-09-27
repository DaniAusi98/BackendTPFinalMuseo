using Core.Application.Repositories;
using Domain.RecursoMuseo.Entities;
using static Domain.ActividadMuseo.Enums.Enums;

namespace Application.MuseumResources.Repositories
{
    public interface IRepositorioConfiguracionSalaActividad : IRepository<ConfiguracionSalaActividad>
    {
        /// <summary>
        /// Obtiene todas las configuraciones de una sala específica
        /// </summary>
        Task<List<ConfiguracionSalaActividad>> ObtenerPorSalaAsync(string salaId);

        /// <summary>
        /// Obtiene todas las configuraciones habilitadas para un tipo de actividad
        /// </summary>
        Task<List<ConfiguracionSalaActividad>> ObtenerPorTipoActividadAsync(TipoActividad tipoActividad);

        /// <summary>
        /// Obtiene una configuración específica de sala y actividad
        /// </summary>
        Task<ConfiguracionSalaActividad?> ObtenerPorSalaYActividadAsync(string salaId, TipoActividad tipoActividad);

        /// <summary>
        /// Elimina todas las configuraciones de una sala
        /// </summary>
        Task EliminarPorSalaAsync(string salaId);

    }
}
