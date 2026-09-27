using Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace Application.Common.DataTransferObjects
{
    public class GeorefLocalidadesResponseDto
    {
        [JsonPropertyName("cantidad")]
        public int Cantidad { get; set; }

        [JsonPropertyName("localidades")]
        public List<LocalidadDto> Localidades { get; set; } = new();
    }

    public class LocalidadDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [JsonPropertyName("departamento_id")]
        public string DepartamentoId { get; set; } = string.Empty;

        [JsonPropertyName("provincia_id")]
        public string ProvinciaId { get; set; } = string.Empty;



        public LocalidadArg MapToDomain()
        {
            return new LocalidadArg(Id,DepartamentoId, ProvinciaId, Nombre);
        }
    }
}
