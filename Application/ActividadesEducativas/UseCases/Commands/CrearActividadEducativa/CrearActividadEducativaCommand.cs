using System.ComponentModel.DataAnnotations;
using Core.Application;
using static Domain.ActividadesAreaEducacion.Enums.Enums;

namespace Application.ActividadesEducativas.UseCases.Commands.CrearActividadEducativa
{
    public class RecursoAsignadoDto
    {
        [Required]
        public string RecursoId { get; set; } = string.Empty;

        [Required]
        public int CantidadAsignada { get; set; }
    }

    public class CrearActividadEducativaCommand : IRequestCommand<string>
    {
        [Required]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        public TipoActividadEducativa TipoActividadEducativa { get; set; }

        [Required]
        public string ProyectoVinculadaId { get; set; } = string.Empty;

        [Required]
        public string InstitucionVinculada { get; set; } = string.Empty;

        [Required]
        public string PublicoObjetivo { get; set; } = string.Empty;
        public string Observaciones { get; set; } = string.Empty;
        // Horario base de la actividad
        [Required]
        public DateTime Inicio { get; set; }
        [Required]
        public DateTime Fin { get; set; }
        public int? CantidadAsistentes { get; set; }
        public List<string> SalasIds { get; set; } = [];
        public bool RequiereDifusion { get; set; }
        public List<string> UrlImagenes { get; set; } = [];
        public bool SolicitarAsistenciaDifusion { get; set; }
        public List<RecursoAsignadoDto> Recursos { get; set; } = [];

        // Recurrencia
        /// <summary>
        /// Regla de recurrencia estándar.
        /// Ejemplo: FREQ=WEEKLY;BYDAY=MO,WE;UNTIL=20261231
        /// Si viene vacío o null, la actividad ocurre una única vez.
        /// </summary>
        public string? RRule { get; set; }

        public CrearActividadEducativaCommand()
        {
        }
    }
}