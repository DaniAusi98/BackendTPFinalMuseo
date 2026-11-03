using Core.Application;
using System.ComponentModel.DataAnnotations;

namespace Application.Eventos.UseCases.Commands.CrearPropuestaEvento
{
    public class PropuestaMuestrasTemporalesCommand : IRequestCommand<string>
    {
        [Required]

        public string NombreApellidoResponsable { get; set; }
        [Required]

        public string EmailResponsable { get; set; }
        [Required]

        public string Telefono { get; set; }
        [Required]

        public string TituloPropuesta { get; set; }
        [Required]

        public LogisticaMuestraTemporalDto LogisticaMuestraTemporalDto { get; set; }
        [Required]
        public DateTime Inicio { get; set; } // Horario de inicio del bloque base

        [Required]
        public DateTime Fin { get; set; }    // Horario de fin del bloque base
        [Required]


        public List<string> SalasIds { get; set; } = [];

    }

    public class LogisticaMuestraTemporalDto
    {
        public DateTime FechaMontaje { get; set; }
        public DateTime FechaMontajeHasta { get; set; }
        public DateTime FechaDesmontaje { get; set; }
        public DateTime FechaDesmontajeHasta { get; set; }

    }
}
