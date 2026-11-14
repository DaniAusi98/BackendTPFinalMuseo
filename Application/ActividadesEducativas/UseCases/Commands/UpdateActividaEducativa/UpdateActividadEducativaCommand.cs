using Application.Eventos.DataTransferObjets;
using Application.MuseumResources.DataTransferObjects;
using Core.Application;
using System.ComponentModel.DataAnnotations;
using static Domain.ActividadesAreaEducacion.Enums.Enums;

namespace Application.ActividadesEducativas.UseCases.Commands.UpdateActividaEducativa
{
    public class UpdateActividadEducativaCommand : IRequestCommand<string>
    {
        [Required]
        public string ActividadEducativaId { get; set; } = string.Empty;

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

        [Required]
        public DateTime Inicio { get; set; }

        [Required]
        public DateTime Fin { get; set; }

        public int? CantidadAsistentes { get; set; }

        public List<string> SalasIds { get; set; } = [];

        public bool RequiereDifusion { get; set; }

        public bool SolicitarAsistenciaDifusion { get; set; }

        public List<string> UrlImagenes { get; set; } = [];

        public List<ArchivoNuevoDto>? ImagenesNuevas { get; set; }

        public List<RecursoAsignadoDto> Recursos { get; set; } = [];

        /// <summary>
        /// Regla de recurrencia estándar.
        /// Si viene vacía o nula, la actividad ocurre una única vez.
        /// </summary>
        public string? RRule { get; set; }

        public UpdateActividadEducativaCommand()
        {
        }
    }
}
