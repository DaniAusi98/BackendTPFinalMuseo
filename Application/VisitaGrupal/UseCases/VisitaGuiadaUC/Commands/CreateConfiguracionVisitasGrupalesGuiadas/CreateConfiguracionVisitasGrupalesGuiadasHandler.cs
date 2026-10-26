
using Application.ApplicationMuseo.Constants;
using Application.ConfiguracionSistema.Repositories;
using Application.Exceptions;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Domain.VisitasGrupales.Entities.GrupalGuiada;
using Domain.VisitasGrupales.ValueObjects;

namespace Application.VisitaGrupal.UseCases.VisitaGuiadaUC.Commands.CreateConfiguracionVisitasGrupalesGuiadas
{
    internal sealed class CreateConfiguracionVisitasGrupalesGuiadasHandler(
        IRepositorioConfiguracionVisitasGrupalesGuiadas repositorio,
        IRepositorioCalendarioMuseo repositorioCalendario)
                : IRequestCommandHandler<CreateConfiguracionVisitasGrupalesGuiadasCommand, string>
    {
        private readonly IRepositorioConfiguracionVisitasGrupalesGuiadas _repositorio = repositorio
                ?? throw new ArgumentNullException(nameof(repositorio));
        private readonly IRepositorioCalendarioMuseo _repositorioCalendario = repositorioCalendario
                ?? throw new ArgumentNullException(nameof(repositorioCalendario));

        public async Task<string> Handle(
            CreateConfiguracionVisitasGrupalesGuiadasCommand request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var calendario = await _repositorioCalendario
                .ObtenerCalendarioActivoAsync()
                ?? throw new EntityDoesNotExistException(
                    "No se encontró el calendario del museo.");

            var diasDisponibles = request.DiasDisponibles
                .Select(h => new HorarioDisponible(
                    h.Dia,
                    h.Desde,
                    h.Hasta))
                .ToList();
            var configuracionActiva = await _repositorio.ObtenerConfiguracionActivaAsync();


            try
            {

                if (configuracionActiva is null)
                {
                    var configuracion = new ConfiguracionVisitasGrupalesGuiadas(
                    capacidadPorGuia: request.CapacidadPorGuia,
                    capacidadMaxima: request.CapacidadMaximaPorTurno,
                    maximoVisitasSimultaneas: request.MaximoVisitasSimultaneas,
                    diasDisponibles: diasDisponibles,
                    calendario: calendario);

                    await _repositorio.AddAsync(configuracion);

                    return configuracion.Id;
                }
                else
                {
                    configuracionActiva.ActualizarConfiguracion(request.CapacidadPorGuia, request.CapacidadMaximaPorTurno, request.MaximoVisitasSimultaneas, diasDisponibles, calendario);

                    _repositorio.Update(configuracionActiva.Id, configuracionActiva);

                    return configuracionActiva.Id;
                }





            }
            catch (Exception ex)
            {
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException);
            }









        }
    }
}


/*
            var configuracion = new ConfiguracionVisitasGrupalesGuiadas(
                capacidadPorGuia: request.CapacidadPorGuia,
                capacidadMaxima: request.CapacidadMaximaPorTurno,
                maximoVisitasSimultaneas: request.MaximoVisitasSimultaneas,
                diasDisponibles: diasDisponibles,
                calendario: calendario);

            await _repositorio.AddAsync(configuracion);

            return configuracion.Id;*/