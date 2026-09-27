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
    internal sealed class ObtenerDepartamentosArg : IConsultaDepartamentos
    {
        private readonly HttpClient _httpClient;

        public ObtenerDepartamentosArg(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }
        public async Task<List<Departamento>> ObtenerDepartamentosArgentinaAsync()
        {
            string url = "georef/api/departamentos?aplanar=true&campos=id%2Cnombre%2Cprovincia.id&max=5000&inicio=0&exacto=true&formato=json";
            GeorefDepartamentosResponseDto? resultadoApi = await _httpClient.GetFromJsonAsync<GeorefDepartamentosResponseDto>(url);

            if (resultadoApi == null || resultadoApi.Departamentos == null)
            {
                return new List<Departamento>();
            }
            return resultadoApi.Departamentos
            .Select(dto => dto.MapToDomain())
            .ToList();
        }
    }
}
