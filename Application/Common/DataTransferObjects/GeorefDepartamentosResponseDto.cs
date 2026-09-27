using Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Common.DataTransferObjects
{
    public class GeorefDepartamentosResponseDto
    {
        [JsonPropertyName("cantidad")]
        public int Cantidad { get; set; }

        [JsonPropertyName("departamentos")]
        public List<DepartamentoDto> Departamentos { get; set; } = new();
    }
    public class DepartamentoDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        
        [JsonPropertyName("provincia_id")]
        public string ProvinciaId { get; set; } = string.Empty;

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;

        public Departamento MapToDomain()
        {
            return new Departamento(Id, ProvinciaId, Nombre);
        }
    }
}
