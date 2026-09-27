using Application.ApplicationMuseo.Constants;
using Application.Exceptions;
using Application.MuseumResources.Repositories;
using Core.Application;
using Domain.Common.Exceptions;
using Domain.RecursoMuseo.Entities;
using static Domain.ActividadMuseo.Enums.Enums;

namespace Application.MuseumResources.UseCases.Commands.ConfigurarSalaActividades
{
    internal sealed class ConfigurarSalaActividadesHandler(
        IRepositorioSala repositorioSala,
        IRepositorioConfiguracionSalaActividad repositorioConfiguracion)
        : IRequestCommandHandler<ConfigurarSalaActividadesCommand, bool>
    {
        private readonly IRepositorioSala _repositorioSala =
            repositorioSala ?? throw new ArgumentNullException(nameof(repositorioSala));

        private readonly IRepositorioConfiguracionSalaActividad _repositorioConfiguracion =
            repositorioConfiguracion ?? throw new ArgumentNullException(nameof(repositorioConfiguracion));

        public async Task<bool> Handle(
            ConfigurarSalaActividadesCommand request,
            CancellationToken cancellationToken)
        {
            // Validar que la sala existe
            var sala = await _repositorioSala.FindOneAsync(request.SalaId);
            if (sala is null)
                throw new BussinessException("La sala no existe.");

            if (request.Configuraciones is null || request.Configuraciones.Count == 0)
                throw new BussinessException("Debe proporcionar al menos una configuración.");

            try
            {
                // Eliminar configuraciones previas de esta sala
                await _repositorioConfiguracion.EliminarPorSalaAsync(request.SalaId);

                // Crear nuevas configuraciones
                foreach (var configDto in request.Configuraciones)
                {
                    var configuracion = new ConfiguracionSalaActividad(
                        salaId: request.SalaId,
                        tipoActividad: (TipoActividad)configDto.TipoActividad,
                        habilitada: configDto.Habilitada,
                        capacidadMaxima: configDto.CapacidadMaxima);

                    await _repositorioConfiguracion.AddAsync(configuracion);
                }

                return true;
            }
            catch (DomainException ex)
            {
                throw new BussinessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex);
            }
        }
    }
}
