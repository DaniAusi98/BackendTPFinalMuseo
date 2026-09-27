using System.ComponentModel.DataAnnotations;
using Core.Application;
using static Domain.Eventos.Enums.Enums;


namespace Application.Eventos.UseCases.Commands
{
    public class RecursoAsignadoDto
    {
        [Required]
        public string RecursoId { get; set; } = string.Empty;

        [Required]
        public int CantidadAsignada { get; set; }
    }

    public class CrearEventoCommand : IRequestCommand<string>
    {
        [Required]
        public string NombreyApellidoSolicitante { get; set; } = string.Empty;

        [Required]
        public string TelefonoSolicitante { get; set; } = string.Empty;

        [Required]
        public string EmailSolicitante { get; set; } = string.Empty;

        [Required]
        public string Institucion { get; set; } = string.Empty;

        public TipoEvento TipoEvento { get; set; }

        [Required]
        public string TituloEvento { get; set; } = string.Empty;

        public string DescripcionEvento { get; set; } = string.Empty;

        public string FundamentacionEvento { get; set; } = string.Empty;

        public List<TipoPublico> TipoPublico { get; set; } = [];

        [Required]
        public int ConcurrenciaEstimada { get; set; }

        [Required]
        public DateTime Inicio { get; set; } // Horario de inicio del bloque base

        [Required]
        public DateTime Fin { get; set; }    // Horario de fin del bloque base

        public List<string> SalasIds { get; set; } = [];

        public bool RequiereDifusion { get; set; }

        public List<RecursoAsignadoDto> Recursos { get; set; } = [];

        public List<string> UrlImagenes { get; set; } = [];
        public bool SolicitarAsistenciaDifusion { get; set; }

        // ============================================================
        // NUEVA RECURRENCIA SIMPLIFICADA
        // ============================================================
        /// <summary>
        /// Recibe la regla de recurrencia estándar directo desde el Front. 
        /// Ejemplo: "FREQ=WEEKLY;BYDAY=TU,TH;UNTIL=20261231"
        /// Si viene nulo o vacío, el evento ocurre una única vez.
        /// </summary>
        public string? RRule { get; set; }

        public CrearEventoCommand()
        {
        }
    }
}
