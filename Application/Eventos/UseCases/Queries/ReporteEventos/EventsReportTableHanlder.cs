
using Application.Availability.ApplicationServices;
using Application.Eventos.DataTransferObjets;
using Application.Eventos.Repositories;
using Core.Application;
using Core.Application.Mapping;
using Domain.Eventos.Entities;

namespace Application.Eventos.UseCases.Queries.ReporteEventos
{
    internal sealed class EventsReportTableHandler(
        IRepositorioEvento repositorioEvento,
        IRecurrenceEvaluator recurrenceEvaluator)
        : IRequestQueryHandler<
            EventsReportTableQuery,
            QueryResult<EventoDto>>
    {
        private readonly IRepositorioEvento _repositorioEvento =
            repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));

        private readonly IRecurrenceEvaluator _recurrenceEvaluator =
            recurrenceEvaluator ?? throw new ArgumentNullException(nameof(recurrenceEvaluator));

        public async Task<QueryResult<EventoDto>> Handle(
            EventsReportTableQuery request,
            CancellationToken cancellationToken)
        {
            var fechaDesde = request.Desde;

            var fechaHasta = (request.Hasta ?? request.Desde)
                .Date
                .AddDays(1)
                .AddSeconds(-1);

            var eventos = await _repositorioEvento.FindAllAsync(
                fechaDesde,
                fechaHasta);

            var ocurrencias = new List<(Evento Evento, DateTime Inicio, DateTime Fin)>();

            foreach (var evento in eventos)
            {
                var duracion = evento.Horario.Fin - evento.Horario.Inicio;

                // Evento sin recurrencia
                if (string.IsNullOrWhiteSpace(evento.RRule))
                {
                    if (evento.Horario.Inicio <= fechaHasta &&
                        evento.Horario.Fin > fechaDesde)
                    {
                        ocurrencias.Add((
                            evento,
                            evento.Horario.Inicio,
                            evento.Horario.Fin));
                    }

                    continue;
                }

                // Evento recurrente
                var slots = _recurrenceEvaluator.ExpandRule(
                    evento.RRule,
                    evento.Horario.Inicio,
                    (int)duracion.TotalMinutes,
                    fechaDesde,
                    fechaHasta);

                foreach (var slot in slots)
                {
                    ocurrencias.Add((
                        evento,
                        slot.Inicio,
                        slot.Fin));
                }
            }

            var resultado = ocurrencias
                .Select(x =>
                {
                    var dto = x.Evento.To<EventoDto>();
                    return dto;
                })
                .ToList();

            return new QueryResult<EventoDto>(
                resultado,
                resultado.Count,
                request.PageIndex,
                request.PageSize);
        }
    }
}
