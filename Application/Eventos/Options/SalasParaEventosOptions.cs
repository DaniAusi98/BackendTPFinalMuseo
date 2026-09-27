using static Domain.RecursoMuseo.Enums.Enums;

namespace Application.Eventos.Options
{
    /// <summary>
    /// Configuración de qué tipos de salas pueden usarse para eventos.
    /// Se carga desde appsettings.json
    /// </summary>
    public class SalasParaEventosOptions
    {
        public const string SectionName = "SalasParaEventos";

        /// <summary>
        /// Lista de tipos de salas permitidas para eventos externos.
        /// Ejemplo: ["ExposicionPermanente", "Auditorio", "Multifuncion"]
        /// </summary>
        public List<string> TiposPermitidos { get; set; } = new();
    }
}
