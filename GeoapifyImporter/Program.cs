
using System.Text.Json;
using GeoapifyImporter.Models;
using GeoapifyImporter.Mappers;

var ruta = @"C:\Users\danie\Documents\Geoapify\geoapify-localities\ar\ar";

var archivos = Directory.GetFiles(ruta, "*.ndjson");

var mapper = new GeoapifyMapper();

int totalRegistros = 0;
int errores = 0;

foreach (var archivo in archivos)
{
    Console.WriteLine($"Leyendo: {Path.GetFileName(archivo)}");

    foreach (var linea in File.ReadLines(archivo))
    {
        if (string.IsNullOrWhiteSpace(linea))
            continue;

        try
        {
            var localidad = JsonSerializer.Deserialize<GeoapifyLocality>(
                linea,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (localidad == null)
                continue;

            mapper.Map(localidad);

            totalRegistros++;
        }
        catch (Exception ex)
        {
            errores++;

            Console.WriteLine();
            Console.WriteLine("===== REGISTRO CON ERROR =====");
            Console.WriteLine($"Archivo: {Path.GetFileName(archivo)}");
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine($"JSON:");
            Console.WriteLine(linea);
            Console.WriteLine("==============================");
            Console.WriteLine();
        }
    }
}

Console.WriteLine();
Console.WriteLine("========================================");
Console.WriteLine("        RESUMEN DE IMPORTACIÓN");
Console.WriteLine("========================================");

Console.WriteLine($"Registros procesados: {totalRegistros}");
Console.WriteLine($"Errores:              {errores}");
Console.WriteLine($"Países:               {mapper.ObtenerPaises().Count}");
Console.WriteLine($"Divisiones:           {mapper.ObtenerDivisiones().Count}");

Console.WriteLine();
Console.WriteLine("========================================");
Console.WriteLine("          PAÍSES DETECTADOS");
Console.WriteLine("========================================");

foreach (var pais in mapper.ObtenerPaises())
{
    Console.WriteLine(
        $"{pais.Nombre} ({pais.Codigo})");
}

Console.WriteLine();
Console.WriteLine("Proceso terminado.");
