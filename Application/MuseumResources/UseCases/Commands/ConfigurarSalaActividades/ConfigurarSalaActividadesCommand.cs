using Core.Application;

namespace Application.MuseumResources.UseCases.Commands.ConfigurarSalaActividades
{
    public class ConfigurarSalaActividadesCommand : IRequestCommand<bool>
    {
        public string SalaId { get; set; } = string.Empty;

        public List<ConfiguracionActivityDto> Configuraciones { get; set; } = [];
    }

    public class ConfiguracionActivityDto
    {
        /// <summary>
        /// TipoActividad como int (0=Evento, 1=VisitaGuiada, etc)
        /// </summary>
        public int TipoActividad { get; set; }

        public bool Habilitada { get; set; }

        public int? CapacidadMaxima { get; set; }
    }
}
