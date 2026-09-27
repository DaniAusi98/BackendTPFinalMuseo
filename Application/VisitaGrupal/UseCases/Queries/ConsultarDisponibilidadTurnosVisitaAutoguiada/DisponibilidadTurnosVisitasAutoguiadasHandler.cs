using Application.Availability.Producers;
using Application.MuseumResources.Repositories;
using Application.VisitaGrupal.DataTransferObjets;
using Application.VisitaGrupal.UseCases.Queries.ConsultarDisponibilidadTurnosVisitaGuiada;
using Core.Application;
using Core.Application.Mapping;

namespace Application.VisitaGrupal.UseCases.Queries.ConsultarDisponibilidadTurnosVisitaAutoguiada
{
    internal sealed class DisponibilidadTurnosVisitasAutoguiadasHandler(
        SelfGuidedAvailabilityProducerService groupAvailabilityProducer,
        AutoMapper.IMapper mapper
       ): IRequestQueryHandler<DisponibilidadTurnosVisitasAutoguiadasQuery,QueryResult<SlotDisponibleDto>>
    {
        private readonly SelfGuidedAvailabilityProducerService _groupAvailabilityProducer =
            groupAvailabilityProducer ?? throw new ArgumentNullException(nameof(groupAvailabilityProducer));

        private readonly AutoMapper.IMapper _mapper =
            mapper ?? throw new ArgumentNullException(nameof(mapper));
    
        public async Task<QueryResult<SlotDisponibleDto>> Handle(DisponibilidadTurnosVisitasAutoguiadasQuery request, CancellationToken cancellationToken)
        {
            var turnosDisponibles = await groupAvailabilityProducer.
                GetHourlyBlocksAsync(
                    request.FechaDesde,
                    request.FechaHasta
                    );
            return new QueryResult<SlotDisponibleDto>(
               turnosDisponibles.To<SlotDisponibleDto>(),
               turnosDisponibles.Count,
               request.PageIndex,
               request.PageSize);
        }
    }
}
