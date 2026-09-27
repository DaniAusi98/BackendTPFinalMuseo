using Application.Common.ApplicationServices;
using Application.Common.DataTransferObjects;
using Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Adapters
{
    internal sealed class ObtenerProvinciasArgentinaGeoRef:IConsultarProvinciasArgetina
    {
        private readonly HttpClient _httpClient;


        public ObtenerProvinciasArgentinaGeoRef(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }
        public async Task<List<Provincia>> ObtenerProvinciasArgentinasAsync()
        {
            // Definimos la URL con los parámetros idénticos a Curl de prueba
            // Esto le indica explícitamente a la API que devuelva solo el set básico
            string url = "georef/api/v2.0/provincias?aplanar=true&campos=basico&inicio=0&formato=json";
            // Deserializamos apuntando al DTO Raíz(GeorefResponseDto)
            GeorefProvinciasResponseDto? resultadoApi = await _httpClient.GetFromJsonAsync<GeorefProvinciasResponseDto>(url);
            if (resultadoApi == null || resultadoApi.Provincias == null)
            {
                return new List<Provincia>();
            }
            return resultadoApi.Provincias
            .Select(dto => dto.MapToDomain())
            .ToList();
        }
    }
}
