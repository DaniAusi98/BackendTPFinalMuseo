using Application.VisitaGrupal.DataTransferObjets;
using Core.Application;

namespace Application.VisitaGrupal.UseCases.VisitaGuiadaUC.Commands.CreateSettingSelfGuidedTours
{
    public class CreateSettingSelfGuidedToursCommand : IRequestCommand<string>

    {

        public List<HorarioDisponibleDto> DiasDisponibles { get; set; }

        public int CapacidadMaximaPorGrupo { get; set; }

        public TimeSpan DuracionVisita { get; set; }

        public TimeSpan IntervaloReservas { get; set; }

        public int VisitasSimultaneasMaximas { get; set; }


    }
}
