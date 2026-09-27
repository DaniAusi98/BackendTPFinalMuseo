using MediatR;

using Core.Application;

namespace Application.VisitaGrupal.UseCases.Commands.CreateConfiguracionVisitasGrupalesGuiadas
{
    public class CreateConfiguracionVisitasGrupalesGuiadasCommand : IRequestCommand<string>
    {
        public int MinGuiasParaCapacidadCompleta { get; set; }
        public int CapacidadPorGuia { get; set; }
        public int CapacidadMaximaPorTurno { get; set; }
        public List<DayOfWeek> DiasDisponibles { get; set; } = new();
        public List<TurnoCommandDto> Turnos { get; set; } = new();
    }

    public class TurnoCommandDto
    {
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
    }
}
