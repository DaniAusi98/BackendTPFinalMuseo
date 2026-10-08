
using Core.Application;

namespace Application.VisitaGrupal.UseCases.Guias.Commands.CrearNoDisponibilidadGuia
{
    public class CrearNoDisponibilidadGuiaCommand : IRequestCommand<string>
    {
        public string GuiaId { get; set; } = default!;
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public string Motivo { get; set; } = default!;
    }
}
