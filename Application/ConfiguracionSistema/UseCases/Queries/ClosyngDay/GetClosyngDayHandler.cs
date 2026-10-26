using Application.ConfiguracionSistema.DataTransferObjets;
using Application.ConfiguracionSistema.Repositories;
using Application.Repositories;
using Core.Application;
using Core.Application.Mapping;
using Domain.Common.Entities;

namespace Application.ConfiguracionSistema.UseCases.Queries.ClosyngDay
{
    internal sealed class GetClosyngDayHandler(IRepositorioCalendarioMuseo repositorioCalendarioMuseo, IRepositorioDiaCierreMuseo repositorioDiaCierreMuseo) : IRequestQueryHandler<GetClosyngDayQuery, List<DiaCierreDto>>
    {
        private readonly IRepositorioCalendarioMuseo _repositorioCalendarioMuseo = repositorioCalendarioMuseo ?? throw new ArgumentNullException(nameof(repositorioCalendarioMuseo));
        private readonly IRepositorioDiaCierreMuseo _repositorioDiaCierreMuseo = repositorioDiaCierreMuseo ?? throw new ArgumentNullException(nameof(repositorioDiaCierreMuseo));
        public async Task<List<DiaCierreDto>> Handle(GetClosyngDayQuery request, CancellationToken cancellationToken)
        {
            List<DiaCierreMuseo> entities = await _repositorioDiaCierreMuseo.FindAllAsync();
            List<DiaCierreDto> diaCierreDtos = entities.To<DiaCierreDto>().ToList();

            return diaCierreDtos;


        }
    }
}
