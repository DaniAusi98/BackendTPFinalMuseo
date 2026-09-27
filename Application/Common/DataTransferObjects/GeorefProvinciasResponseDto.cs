namespace Application.Common.DataTransferObjects;

using Domain.Common.Entities;
using System.Text.Json.Serialization;

public class GeorefProvinciasResponseDto
{
    [JsonPropertyName("cantidad")]
    public int Cantidad { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonPropertyName("provincias")]
    public List<ProvinciaDto> Provincias { get; set; } = new();
}

public class ProvinciaDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = string.Empty;

    public Provincia MapToDomain()
    {
        return new Provincia(Id, Nombre);
    }
}
