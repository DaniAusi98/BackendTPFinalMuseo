using Application.ActividadesEducativas.DataTransferObjets;
using Application.ActividadesEducativas.Repositories;
using Application.Availability.ApplicationServices;
using Core.Application;
using Core.Application.Mapping;
using Domain.ActividadesAreaEducacion.Entities;

namespace Application.ActividadesEducativas.UseCases.Queries.GetActividadesEducativas
{
    internal sealed class ActividadesEducativasHandler(IRepositorioActividadEducativa repositorioActividadEducativa,
        IRecurrenceEvaluator recurrenceEvaluator) :
        IRequestQueryHandler<ActividadesEducativasQuery, QueryResult<ActividadEducativaDto>>
    {
        private readonly IRepositorioActividadEducativa _repositorioActividadEducativa = repositorioActividadEducativa ?? throw new ArgumentNullException(nameof(repositorioActividadEducativa));
        private readonly IRecurrenceEvaluator _recurrenceEvaluator = recurrenceEvaluator ?? throw new ArgumentNullException(nameof(recurrenceEvaluator));

        public async Task<QueryResult<ActividadEducativaDto>> Handle(ActividadesEducativasQuery request, CancellationToken cancellationToken)
        {
            var actividadesEducacion = await _repositorioActividadEducativa.GetAllActEducationAsync(request.Desde, request.Hasta);
            var ocurrencias = ObtenerOcurrencias(actividadesEducacion, request.Desde, request.Hasta);

            var actividadesEducativaDtos = ocurrencias.Select(MapearActividadConHorario).ToList();
            int cantidad = actividadesEducativaDtos.Count;
            return new QueryResult<ActividadEducativaDto>(
                actividadesEducativaDtos,
                cantidad,
                request.PageIndex,
                request.PageSize);


        }

        private List<(ActividadEducativa Actividad, DateTime Inicio, DateTime Fin)> ObtenerOcurrencias(
        IEnumerable<ActividadEducativa> actividades, DateTime desde, DateTime hasta)
        {
            var lista = new List<(ActividadEducativa, DateTime, DateTime)>();

            foreach (var actEduc in actividades)
            {
                var duracion = actEduc.Horario.Fin - actEduc.Horario.Inicio;

                if (string.IsNullOrWhiteSpace(actEduc.RRule))
                {
                    if (actEduc.Horario.Inicio < hasta && actEduc.Horario.Fin > desde)
                    {
                        lista.Add((actEduc, actEduc.Horario.Inicio, actEduc.Horario.Fin));
                    }
                    continue;
                }

                var slots = _recurrenceEvaluator.ExpandRule(actEduc.RRule, actEduc.Horario.Inicio, (int)duracion.TotalMinutes, desde, hasta);
                foreach (var slot in slots)
                {
                    lista.Add((actEduc, slot.Inicio, slot.Fin));
                }
            }

            return lista;
        }

        private ActividadEducativaDto MapearActividadConHorario((ActividadEducativa Actividad, DateTime Inicio, DateTime Fin) ocurrencia)
        {
            var dto = ocurrencia.Actividad.To<ActividadEducativaDto>();
            dto.TimeSlot.Inicio = ocurrencia.Inicio;
            dto.TimeSlot.Fin = ocurrencia.Fin;
            return dto;
        }
    }
}