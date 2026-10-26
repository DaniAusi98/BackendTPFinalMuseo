
using Application.ConfiguracionSistema.DataTransferObjets;
using Application.ConfiguracionSistema.Repositories;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Adapters.Feriados
{
    internal sealed class ArgentinaDatosFeriadosAdapter : IFeriadosProvider
    {
        private readonly HttpClient _httpClient;

        public ArgentinaDatosFeriadosAdapter(HttpClient httpClient)
        {
            _httpClient = httpClient
                ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<IReadOnlyCollection<FeriadoExternoDto>>
            ObtenerFeriadosAsync(
                int anio,
                CancellationToken cancellationToken)
        {
            if (anio < 2016 || anio > DateTime.Now.Year)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(anio),
                    $"El año debe estar entre 2016 y {DateTime.Now.Year}.");
            }

            var respuesta = await _httpClient
                .GetFromJsonAsync<List<FeriadoApiDto>>(
                    $"v1/feriados/{anio}",
                    cancellationToken);

            if (respuesta is null)
            {
                throw new InvalidOperationException(
                    "La API de feriados devolvió una respuesta vacía.");
            }

            return respuesta
                .Select(f => new FeriadoExternoDto(
                    Fecha: DateOnly.Parse(f.Fecha),
                    Nombre: f.Nombre,
                    Ambito: AmbitoFeriado.Nacional,
                    Tipo: MapearTipo(f.Tipo)))
                .ToList();
        }

        private static TipoFeriado MapearTipo(string? tipo)
        {
            if (string.IsNullOrWhiteSpace(tipo))
                return TipoFeriado.Otro;

            return tipo.Trim().ToLowerInvariant() switch
            {
                "inamovible" => TipoFeriado.Inamovible,
                "trasladable" => TipoFeriado.Trasladable,
                "nolaborable" or "no laborable" =>
                    TipoFeriado.NoLaborable,
                _ => TipoFeriado.Otro
            };
        }

        private sealed record FeriadoApiDto(
            [property: JsonPropertyName("fecha")]
            string Fecha,

            [property: JsonPropertyName("tipo")]
            string Tipo,

            [property: JsonPropertyName("nombre")]
            string Nombre);
    }
}
