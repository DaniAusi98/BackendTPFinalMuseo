using Application.ApplicationMuseo.Constants;
using Application.ConfiguracionSistema.Repositories;
using Application.Exceptions;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Domain.VisitasGrupales.Entities.GrupalAutoguiada;
using Domain.VisitasGrupales.ValueObjects;

namespace Application.VisitaGrupal.UseCases.Commands.CreateSettingSelfGuidedTours
{
    internal sealed class CreateSettingSelfGuidedToursHandler(
        IRepositorioCalendarioMuseo repositorioCalendario,
        IRepositorioConfiguracionHorarioAutoguiada configuracionHorarioAutoguiada
        ) : IRequestCommandHandler<CreateSettingSelfGuidedToursCommand, string>
    {
        private readonly IRepositorioConfiguracionHorarioAutoguiada _repositorio = configuracionHorarioAutoguiada
                ?? throw new ArgumentNullException(nameof(configuracionHorarioAutoguiada));
        private readonly IRepositorioCalendarioMuseo _repositorioCalendario = repositorioCalendario
                ?? throw new ArgumentNullException(nameof(repositorioCalendario));
        public async Task<string> Handle(CreateSettingSelfGuidedToursCommand request, CancellationToken cancellationToken)
        {
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
                    var configuracion = new ConfiguracionHorarioAutoguiada(
                    request.CapacidadMaximaPorGrupo,
                    diasDisponibles,
                    calendario,
                    request.DuracionVisita,
                    request.IntervaloReservas,
                    request.VisitasSimultaneasMaximas
                    );

                    await _repositorio.AddAsync(configuracion);

                    return configuracion.Id;
                }
                else
                {
                    configuracionActiva.ActualizarConfiguracion
                   (request.CapacidadMaximaPorGrupo,
                   request.DuracionVisita,
                    request.IntervaloReservas,
                    request.VisitasSimultaneasMaximas,
                    diasDisponibles,
                    calendario
                 );

                    _repositorio.Update(configuracionActiva.Id, configuracionActiva);

                    return configuracionActiva.Id;
                }





            }
            catch (Exception ex)
            {
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException);
            }
            throw new NotImplementedException();
        }
    }
}
