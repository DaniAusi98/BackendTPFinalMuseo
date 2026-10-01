using Application.ActividadesEducativas.DataTransferObjets;
using Application.ActividadesEducativas.Repositories;
using Application.Availability.ApplicationServices;
using Application.VisitaGrupal.DataTransferObjets;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Core.Application.Mapping;
using Domain.ActividadesAreaEducacion.Entities;

namespace Application.ActividadesEducativas.UseCases.Queries.GetActEducacion;

internal sealed class ActividadesEducacionHandler(
    IRepositorioActividadEducativa repositorioActividadEducativa,
    IRepositorioVisitaGuiada repositorioVisitaGuiada,
    IRecurrenceEvaluator recurrenceEvaluator) :
    IRequestQueryHandler<ActividadesEducacionQuery, CalendarEducacionDto>
{

    public async Task<CalendarEducacionDto> Handle(ActividadesEducacionQuery request, CancellationToken cancellationToken)
    {
        var visitasGuiadas = await repositorioVisitaGuiada.GetAllGroupVisitAsync(request.Desde, request.Hasta);
        var actividadesEducacion = await repositorioActividadEducativa.GetAllActEducationAsync(request.Desde, request.Hasta);

        var visitasGuiadasDtos = visitasGuiadas.To<GuidedTourReservationDto>().ToList();

        var ocurrencias = ObtenerOcurrencias(actividadesEducacion, request.Desde, request.Hasta);

        var actividadesEducativaDtos = ocurrencias.Select(MapearActividadConHorario).ToList();

        return new CalendarEducacionDto
        {
            VisitasGuiadas = visitasGuiadasDtos,
            ActividadesEducativas = actividadesEducativaDtos
        };
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

            var slots = recurrenceEvaluator.ExpandRule(actEduc.RRule, actEduc.Horario.Inicio, (int)duracion.TotalMinutes, desde, hasta);
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
