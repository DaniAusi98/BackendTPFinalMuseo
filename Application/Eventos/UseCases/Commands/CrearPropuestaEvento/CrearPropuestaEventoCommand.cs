using Application.MuseumResources.DataTransferObjects;
using Core.Application;
using static Domain.Eventos.Enums.Enums;

namespace Application.Eventos.UseCases.Commands.CrearPropuestaEvento
{
    public class CrearPropuestaEventoCommand : IRequestCommand<string>
    {
        public string NombreyApellidoResponsable { get; set; }
        public string TelefonoResponsable { get; set; }
        public string EmailResponsable { get; set; }
        public string TituloEvento { get; set; }
        public TipoEvento TipoEvento { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public List<string> SalasIds { get; set; } = [];
        public List<RecursoAsignadoDto> Recursos { get; set; } = [];
        public string? RRule { get; set; }

        public CrearPropuestaEventoCommand()
        {

        }






    }
}
