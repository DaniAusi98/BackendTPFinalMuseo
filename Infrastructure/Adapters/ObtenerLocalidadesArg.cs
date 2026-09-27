using Application.Common.ApplicationServices;
using Application.Common.DataTransferObjects;
using Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Adapters
{
    internal sealed class ObtenerLocalidadesArg : IConsultarLocalidades
    {
        private readonly HttpClient _httpClient;
        public ObtenerLocalidadesArg(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<LocalidadArg>> ObtenerLocalidadesArgentinaAsync()
        {
            string url = "georef/api/localidades?aplanar=true&campos=id%2Cnombre%2Cdepartamento.id%2Cprovincia.id&max=5000&inicio=0&exacto=true&formato=json";
            GeorefLocalidadesResponseDto? response = await _httpClient.GetFromJsonAsync<GeorefLocalidadesResponseDto>(url);

            if (response == null || response.Localidades == null)
            {
                return new List<LocalidadArg>();
            }
            return response.Localidades
            .Select(dto => dto.MapToDomain())
            .ToList();
        }
    }
}