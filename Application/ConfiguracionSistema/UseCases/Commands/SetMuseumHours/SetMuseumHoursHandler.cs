using Application.ApplicationMuseo.Constants;
using Application.ConfiguracionSistema.Repositories;
using Application.Exceptions;
using Core.Application;
using Domain.Common.Entities;
using Domain.Common.ValueObjets;

namespace Application.ConfiguracionSistema.UseCases.Commands.SetMuseumHours
{
    internal sealed class SetMuseumHoursHandler(IRepositorioCalendarioMuseo repositorioCalendarioMuseo) : IRequestCommandHandler<SetMuseumHoursCommand, string>
    {
        private readonly IRepositorioCalendarioMuseo _repositorioCalendarioMuseo = repositorioCalendarioMuseo ?? throw new ArgumentNullException(nameof(repositorioCalendarioMuseo));
        public async Task<string> Handle(SetMuseumHoursCommand request, CancellationToken cancellationToken)
        {
            var horariosApertura = request.HorariosApertura
                 .Select(h => new HorarioApertura(
                     h.Dia,
                     h.Desde,
                     h.Hasta))
                 .ToList();

            try
            {
                var configuracion = await _repositorioCalendarioMuseo.ObtenerCalendarioActivoAsync();

                if (configuracion is null)
                {
                    var nuevaConfiguracion = new HorarioAperturaMuseo(horariosApertura);


                    object createdId = await _repositorioCalendarioMuseo.AddAsync(nuevaConfiguracion);
                    return createdId.ToString();
                }
                else
                {
                    configuracion.ActualizarHorario(horariosApertura);

                    _repositorioCalendarioMuseo.Update(configuracion.Id, configuracion);

                    return configuracion.Id;
                }





            }
            catch (Exception ex)
            {
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException);
            }

        }
    }
}
