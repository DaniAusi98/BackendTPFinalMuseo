using Core.Application;

namespace Application.VisitaGrupal.UseCases.VisitaGuiadaUC.Commands.CreateConfiguracionVisitasGrupalesGuiadas
{
    public class CreateConfiguracionVisitasGrupalesGuiadasCommand : IRequestCommand<string>
    {
        public int CapacidadPorGuia { get; set; } = 25;

        public int CapacidadMaximaPorTurno { get; set; } = 50;

        public int MaximoVisitasSimultaneas { get; set; } = 1;

        public List<HorarioDisponibleCommandDto> DiasDisponibles { get; set; } = new();
    }

    public class HorarioDisponibleCommandDto
    {
        public DayOfWeek Dia { get; set; }

        public TimeOnly Desde { get; set; }

        public TimeOnly Hasta { get; set; }
    }
}