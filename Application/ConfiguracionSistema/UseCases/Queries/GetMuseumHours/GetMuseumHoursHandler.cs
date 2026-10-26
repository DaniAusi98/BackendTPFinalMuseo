using Application.ConfiguracionSistema.DataTransferObjets;
using Application.ConfiguracionSistema.Repositories;
using Core.Application;
using Core.Application.Mapping;
using Domain.Common.Entities;

namespace Application.ConfiguracionSistema.UseCases.Queries.GetMuseumHours
{
    internal sealed class GetMuseumHoursHandler(IRepositorioCalendarioMuseo repositorioCalendarioMuseo) : IRequestQueryHandler<GetMuseumHoursQuery, MuseumHoursDto>
    {
        private readonly IRepositorioCalendarioMuseo _repositorioCalendarioMuseo = repositorioCalendarioMuseo ?? throw new ArgumentNullException(nameof(repositorioCalendarioMuseo));
        public async Task<MuseumHoursDto> Handle(GetMuseumHoursQuery request, CancellationToken cancellationToken)
        {
            HorarioAperturaMuseo horarioAperturaMuseo = await _repositorioCalendarioMuseo.ObtenerCalendarioActivoAsync();

            List<HorarioAperturaDto> horarioAperturaDtos = [.. horarioAperturaMuseo.HorariosApertura.To<HorarioAperturaDto>()];
            List<DiaCierreDto> diaCierreDtos = [.. horarioAperturaMuseo.DiasCierre.To<DiaCierreDto>()];
            var resultado = new MuseumHoursDto(horarioAperturaDtos, diaCierreDtos);

            return resultado;



        }
    }
}
