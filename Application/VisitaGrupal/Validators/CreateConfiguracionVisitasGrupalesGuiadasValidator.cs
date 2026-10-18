using Application.VisitaGrupal.UseCases.VisitaGuiadaUC.Commands.CreateConfiguracionVisitasGrupalesGuiadas;
using FluentValidation;

namespace Application.VisitaGrupal.Validators
{
    public class CreateConfiguracionVisitasGrupalesGuiadasValidator
        : AbstractValidator<CreateConfiguracionVisitasGrupalesGuiadasCommand>
    {
        public CreateConfiguracionVisitasGrupalesGuiadasValidator()
        {
            RuleFor(x => x.CapacidadPorGuia)
                .GreaterThan(0)
                .WithMessage("La capacidad por guía debe ser mayor a 0.");

            RuleFor(x => x.CapacidadMaximaPorTurno)
                .GreaterThan(0)
                .WithMessage("La capacidad máxima por turno debe ser mayor a 0.")
                .GreaterThanOrEqualTo(x => x.CapacidadPorGuia)
                .WithMessage(
                    "La capacidad máxima no puede ser menor a la capacidad por guía.");

            RuleFor(x => x.MaximoVisitasSimultaneas)
                .GreaterThan(0)
                .WithMessage("El máximo de visitas simultáneas debe ser mayor a 0.");

            RuleFor(x => x.DiasDisponibles)
                .NotNull()
                .WithMessage("Debe especificar los horarios disponibles.")
                .Must(horarios => horarios != null && horarios.Any())
                .WithMessage("Debe existir al menos un horario disponible.");

            RuleForEach(x => x.DiasDisponibles)
                .ChildRules(horario =>
                {
                    horario.RuleFor(h => h.Hasta)
                        .GreaterThan(h => h.Desde)
                        .WithMessage(
                            "La hora de fin debe ser posterior a la hora de inicio.");

                    horario.RuleFor(h => h)
                        .Must(h =>
                        {
                            var duracion = h.Hasta.ToTimeSpan()
                                - h.Desde.ToTimeSpan();

                            return duracion.TotalMinutes >= 30;
                        })
                        .WithMessage(
                            "Un horario debe durar al menos 30 minutos.");
                });

            RuleFor(x => x.DiasDisponibles)
                .Must(horarios =>
                {
                    if (horarios == null || horarios.Count <= 1)
                        return true;

                    for (int i = 0; i < horarios.Count; i++)
                    {
                        for (int j = i + 1; j < horarios.Count; j++)
                        {
                            var h1 = horarios[i];
                            var h2 = horarios[j];

                            if (h1.Dia == h2.Dia &&
                                h1.Desde < h2.Hasta &&
                                h1.Hasta > h2.Desde)
                            {
                                return false;
                            }
                        }
                    }

                    return true;
                })
                .WithMessage(
                    "Los horarios disponibles no deben solaparse en un mismo día.");
        }
    }
}