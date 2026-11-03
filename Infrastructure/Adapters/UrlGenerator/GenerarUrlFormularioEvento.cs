using Application.Eventos.ApplicationServices;
using Microsoft.Extensions.Configuration;
using static Domain.Eventos.Enums.Enums;

namespace Infrastructure.Adapters.UrlGenerator
{
    public class UrlFormularioEvento(IConfiguration configuration) : IGenerarUrlFormularioEvento
    {
        private readonly IConfiguration _configuration = configuration;

        public string GenerarUrlFormularioEvento(
            string EventoId,
            TipoEvento tipoEvento)
        {

            string ruta;

            if (tipoEvento == TipoEvento.MuestraExposicionTemporal)
            {
                ruta = "muestras/formulario";
            }
            else if (Enum.IsDefined(typeof(TipoEvento), tipoEvento))
            {
                ruta = "eventos/formulario";
            }
            else
            {
                // Si no es ninguno de los anteriores, lanzar excepción
                throw new ArgumentOutOfRangeException(nameof(tipoEvento), $"El estado {tipoEvento} no está soportado.");
            }

            var frontendUrl = _configuration["Frontend:BaseUrl"];

            return $"{frontendUrl}/{ruta}/{EventoId}";


        }

    }
}
