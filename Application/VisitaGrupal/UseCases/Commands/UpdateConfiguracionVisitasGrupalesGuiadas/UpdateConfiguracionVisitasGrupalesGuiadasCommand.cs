using System.ComponentModel.DataAnnotations;
using Core.Application;

namespace Application.VisitaGrupal.UseCases.Commands.UpdateConfiguracionVisitasGrupalesGuiadas
{
    public class UpdateConfiguracionVisitasGrupalesGuiadasCommand : IRequestCommand
    {
        [Required]
        public int MinGuiasParaCapacidadCompleta { get; set; }
        [Required]
        public int CapacidadPorGuia { get; set; }
        [Required]
        public int CapacidadMaximaPorTurno { get; set; }
        [Required]
        public List<DayOfWeek> DiasDisponibles { get; set; } = new();
        [Required]
        public List<TurnoUpdateDto> Turnos { get; set; } = new();
    }

    public class TurnoUpdateDto
    {
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
    }
}
