using FluentValidation;

namespace Application.ConfiguracionSistema.UseCases.Commands.SetMuseumHours
{
    public class SetMuseumHoursCommandValidator
        : AbstractValidator<SetMuseumHoursCommand>
    {
        public SetMuseumHoursCommandValidator()
        {
            RuleFor(x => x.HorariosApertura)
                .NotNull()
                .NotEmpty()
                .WithMessage("Debe indicar al menos un horario de apertura.");

            RuleForEach(x => x.HorariosApertura)
                .ChildRules(horario =>
                {
                    horario.RuleFor(x => x.Desde)
                        .LessThan(x => x.Hasta)
                        .WithMessage(
                            "La hora de apertura debe ser anterior a la hora de cierre.");
                });

            RuleFor(x => x.HorariosApertura)
                .Must(horarios => horarios == null ||
                    horarios.GroupBy(h => h.Dia)
                        .All(grupo =>
                        {
                            var ordenados = grupo
                                .OrderBy(h => h.Desde)
                                .ToList();

                            for (int i = 1; i < ordenados.Count; i++)
                            {
                                if (ordenados[i].Desde < ordenados[i - 1].Hasta)
                                    return false;
                            }

                            return true;
                        }))
                .WithMessage(
                    "Los horarios de un mismo día no pueden solaparse.");
        }
    }
}