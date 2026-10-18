using Core.Application;

namespace Application.VisitaGrupal.UseCases.Guias.Commands.CrearAusenciaGuia
{
    public class CrearAusenciaCommand : IRequestCommand<string>
    {
        public string GuiaId { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public string Motivo { get; set; }

    }
}
