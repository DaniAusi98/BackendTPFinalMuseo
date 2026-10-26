using Application.Eventos.ApplicationServices;
using Microsoft.Extensions.Configuration;
using static Domain.Eventos.Enums.Enums;

namespace Infrastructure.Adapters.UrlGenerator
{
    public class UrlFormularioEvento(IConfiguration configuration) : IGenerarUrlFormularioEvento
    {
        private readonly IConfiguration _configuration = configuration;

        public string GenerarUrlFormularioEvento(
            string propuestaId,
            TipoEventoActividad tipoEvento)
        {

            string ruta = tipoEvento switch
            {
                TipoEventoActividad.EventoActividadExterna => "eventos/formulario",

                TipoEventoActividad.MuestraExposicionTemporal => "muestras/formulario",

                _ => throw new ArgumentException(
                    "El tipo de propuesta no tiene un formulario asociado.")
            };
            var frontendUrl = _configuration["Frontend:BaseUrl"];

            return $"{frontendUrl}/{ruta}/{propuestaId}";


        }

    }
}
