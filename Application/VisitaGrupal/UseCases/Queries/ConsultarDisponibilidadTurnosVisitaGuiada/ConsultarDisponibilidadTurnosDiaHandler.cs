using Application.Availability.Producers;
using Application.VisitaGrupal.DataTransferObjets;
using Core.Application;
using Core.Application.Mapping;

namespace Application.VisitaGrupal.UseCases.Queries.ConsultarDisponibilidadTurnosVisitaGuiada
{
    internal class ConsultarDisponibilidadTurnosDiaHandler(
        GuidedAvailabilityProducerService guidedAvailabilityProducer,
        AutoMapper.IMapper mapper)
        : IRequestQueryHandler<
            ConsultarDisponibilidadTurnosDiaQuery,
            QueryResult<TurnoDisponibleDto>>
    {
        private readonly GuidedAvailabilityProducerService _guidedAvailabilityProducer =
            guidedAvailabilityProducer ?? throw new ArgumentNullException(nameof(guidedAvailabilityProducer));

        private readonly AutoMapper.IMapper _mapper =
            mapper ?? throw new ArgumentNullException(nameof(mapper));

        public async Task<QueryResult<TurnoDisponibleDto>> Handle(
      ConsultarDisponibilidadTurnosDiaQuery request,
      CancellationToken cancellationToken)
        {
            var desde = request.FechaDesde.Date;
            var hasta = request.FechaHasta.Date;

            var turnosDisponibles = await _guidedAvailabilityProducer
                .GetAvailableTurnsAsync(desde, hasta);

            return new QueryResult<TurnoDisponibleDto>(
                turnosDisponibles.To<TurnoDisponibleDto>(),
                turnosDisponibles.Count,
                request.PageIndex,
                request.PageSize);
        }
    }
}