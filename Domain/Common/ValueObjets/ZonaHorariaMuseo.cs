using System.Runtime.InteropServices;
using TimeZoneConverter;

namespace Domain.Common.ValueObjets
{
    public record ZonaHoraria
    {
        public string Id { get; init; }

        private ZonaHoraria(string id)
        {
            Id = id;
        }

        public static ZonaHoraria Crear(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("La zona horaria no puede estar vacía.");
            }

            string trimmedId = id.Trim();

            try
            {
                // 1. Intento Nativo: Validar con el sistema operativo
                TimeZoneInfo.FindSystemTimeZoneById(trimmedId);
            }
            catch (TimeZoneNotFoundException)
            {
                try
                {
                    // 2. Intento Nativo Avanzado: Traducir según el OS actual (Requiere ICU)
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) &&
                        TimeZoneInfo.TryConvertIanaIdToWindowsId(trimmedId, out _))
                    {
                        return new ZonaHoraria(trimmedId);
                    }

                    if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows) &&
                        TimeZoneInfo.TryConvertWindowsIdToIanaId(trimmedId, out _))
                    {
                        return new ZonaHoraria(trimmedId);
                    }
                }
                catch (Exception)
                {
                    // Si el OS no soporta estos métodos (Server 2016), pasamos silenciosamente al plan C
                }

                // 3. Última Instancia (Rescate): Si el OS falló o es Server 2016 sin ICU
                if (!TZConvert.KnownIanaTimeZoneNames.Contains(trimmedId) &&
                    !TZConvert.KnownWindowsTimeZoneIds.Contains(trimmedId))
                {
                    throw new ArgumentException($"La zona horaria '{trimmedId}' no es válida en ninguna plataforma.");
                }
            }

            return new ZonaHoraria(trimmedId);
        }

        public TimeZoneInfo ToTimeZoneInfo()
        {
            try
            {
                // Paso 1: Intentar de la forma nativa del sistema operativo
                return TimeZoneInfo.FindSystemTimeZoneById(Id);
            }
            catch (TimeZoneNotFoundException)
            {
                try
                {
                    // Paso 2: Intentar traducción nativa multiplataforma
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) &&
                        TimeZoneInfo.TryConvertIanaIdToWindowsId(Id, out string? windowsId))
                    {
                        return TimeZoneInfo.FindSystemTimeZoneById(windowsId);
                    }

                    if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows) &&
                        TimeZoneInfo.TryConvertWindowsIdToIanaId(Id, out string? ianaId))
                    {
                        return TimeZoneInfo.FindSystemTimeZoneById(ianaId);
                    }
                }
                catch (Exception)
                {
                    // Captura errores si los métodos nativos no están instalados en el OS
                }

                // Paso 3: Plan de contingencia definitivo (Server 2016 sin ICU)
                return TZConvert.GetTimeZoneInfo(Id);
            }
        }
    }
}
