using Application.ActividadMuseo.DataTransferObjets;
using Application.ActividadMuseo.Repositories;
using Application.Availability.ApplicationServices;
using Core.Application;
using Core.Application.Mapping;


namespace Application.ActividadMuseo.UseCases.ActividadesMuseo.Queries
{
    internal sealed class GetCalendarActivitiesHandler(
        IRepositorioActividadMuseo repositorioActividad,
        IRecurrenceEvaluator recurrenceEvaluator)
        : IRequestQueryHandler<GetAllActivitiesQuery, QueryResult<ActividadMuseoDto>>
    {
        public async Task<QueryResult<ActividadMuseoDto>> Handle(GetAllActivitiesQuery request, CancellationToken cancellationToken)
        {
            var entities = await repositorioActividad.FindAllCalendarAsync(request.FechaConsultaDesde, request.FechaConsultaHasta);

            var ocurrencias = ObtenerOcurrencias(entities, request.FechaConsultaDesde, request.FechaConsultaHasta);

            var actividadesDtos = ocurrencias.Select(MapearActividadConHorario).ToList();

            return new QueryResult<ActividadMuseoDto>(actividadesDtos, ocurrencias.Count, request.PageIndex, request.PageSize);
        }

        private List<(Domain.ActividadMuseo.Entities.ActividadMuseo Actividad, DateTime Inicio, DateTime Fin)> ObtenerOcurrencias(
            IEnumerable<Domain.ActividadMuseo.Entities.ActividadMuseo> actividades, DateTime desde, DateTime hasta)
        {
            var lista = new List<(Domain.ActividadMuseo.Entities.ActividadMuseo, DateTime, DateTime)>();

            foreach (var actividad in actividades)
            {
                var duracion = actividad.Horario.Fin - actividad.Horario.Inicio;

                if (string.IsNullOrWhiteSpace(actividad.RRule))
                {
                    if (actividad.Horario.Inicio < hasta && actividad.Horario.Fin > desde)
                    {
                        lista.Add((actividad, actividad.Horario.Inicio, actividad.Horario.Fin));
                    }
                    continue;
                }

                var slots = recurrenceEvaluator.ExpandRule(
                    actividad.RRule,
                    actividad.Horario.Inicio,
                    (int)duracion.TotalMinutes,
                    desde,
                    hasta);

                foreach (var slot in slots)
                {
                    lista.Add((actividad, slot.Inicio, slot.Fin));
                }
            }

            return lista;
        }

        private ActividadMuseoDto MapearActividadConHorario((Domain.ActividadMuseo.Entities.ActividadMuseo Actividad, DateTime Inicio, DateTime Fin) ocurrencia)
        {
            var dto = ocurrencia.Actividad.To<ActividadMuseoDto>();
            dto.Horario.Inicio = ocurrencia.Inicio;
            dto.Horario.Fin = ocurrencia.Fin;
            return dto;
        }
    }
}