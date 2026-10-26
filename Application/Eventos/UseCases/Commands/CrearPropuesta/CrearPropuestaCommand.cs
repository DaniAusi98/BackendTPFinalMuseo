using Application.MuseumResources.DataTransferObjects;
using Core.Application;
using static Domain.Eventos.Enums.Enums;

namespace Application.Eventos.UseCases.Commands.CrearPropuesta
{
    public class CrearPropuestaCommand : IRequestCommand<string>
    {
        public string NombreApellidoResponsable { get; set; }
        public string TelefonoResponsable { get; set; }
        public string EmailResponsable { get; set; }
        public string TituloPropuesta { get; set; }
        public TipoEventoActividad TipoEvento { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaHasta { get; set; }
        public List<string> SalasIds { get; set; } = [];
        public List<RecursoAsignadoDto> Recursos { get; set; } = [];

        public string? RRule { get; set; }

        public CrearPropuestaCommand()
        {

        }

    }
}

