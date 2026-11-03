using Application.Eventos.DataTransferObjets;

namespace Application.Common.ApplicationServices;

public interface IStorageService
{
    Task<string> GuardarArchivoAsync(ArchivoNuevoDto archivo, string carpeta, CancellationToken cancellationToken);
    void EliminarArchivo(string urlArchivo, string carpeta);
}
