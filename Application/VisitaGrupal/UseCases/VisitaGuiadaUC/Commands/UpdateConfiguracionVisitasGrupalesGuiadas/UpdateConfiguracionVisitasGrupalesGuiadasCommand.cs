using Core.Application;
using System.ComponentModel.DataAnnotations;

namespace Application.VisitaGrupal.UseCases.VisitaGuiadaUC.Commands.UpdateConfiguracionVisitasGrupalesGuiadas
{
    public class UpdateConfiguracionVisitasGrupalesGuiadasCommand : IRequestCommand
    {
        [Required]
        public int CapacidadPorGuia { get; set; } = 25;

        [Required]
        public int CapacidadMaximaPorTurno { get; set; } = 50;

        [Required]
        public int MaximoVisitasSimultaneas { get; set; } = 1;

        [Required]
        public List<HorarioDisponibleUpdateDto> DiasDisponibles { get; set; } = new();
    }

    public class HorarioDisponibleUpdateDto
    {
        public DayOfWeek Dia { get; set; }

        public TimeOnly Desde { get; set; }

        public TimeOnly Hasta { get; set; }
    }
}