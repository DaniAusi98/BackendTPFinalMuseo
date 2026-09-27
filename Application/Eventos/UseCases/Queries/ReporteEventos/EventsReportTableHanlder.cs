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
            QueryResult<TablaReporteEventoDto>>
    {
        private readonly IRepositorioEvento _repositorioEvento =
            repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));

        private readonly IRecurrenceEvaluator _recurrenceEvaluator =
            recurrenceEvaluator ?? throw new ArgumentNullException(nameof(recurrenceEvaluator));

        public async Task<QueryResult<TablaReporteEventoDto>> Handle(
            EventsReportTableQuery request,
            CancellationToken cancellationToken)
        {
            var eventos = await _repositorioEvento.FindAllAsync(
                request.Desde,
                request.Hasta);

            var ocurrencias = new List<(Evento Evento, DateTime Inicio, DateTime Fin)>();

            foreach (var evento in eventos)
            {
                var duracion = evento.Horario.Fin - evento.Horario.Inicio;

                // Evento sin recurrencia
                if (string.IsNullOrWhiteSpace(evento.RRule))
                {
                    if (evento.Horario.Inicio < request.Hasta &&
                        evento.Horario.Fin > request.Desde)
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
                    request.Desde,
                    request.Hasta);

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
                    var dto = x.Evento.To<TablaReporteEventoDto>();

                    dto.FechaInicio = x.Inicio;
                    dto.FechaFin = x.Fin;

                    return dto;
                })
                .ToList();

            return new QueryResult<TablaReporteEventoDto>(
                resultado,
                resultado.Count,
                request.PageIndex,
                request.PageSize);
        }
    }
}