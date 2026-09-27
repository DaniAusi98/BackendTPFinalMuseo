using FluentValidation;
using Application.VisitaGrupal.DataTransferObjets;

namespace Application.VisitaGrupal.UseCases.Comands.CreateGuia
{
    public class CreateGuiaCommandValidator : AbstractValidator<CreateGuiaCommand>
    {
        public CreateGuiaCommandValidator()
        {
            RuleFor(x => x.NombreCompleto)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.PersonalInternoId)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Horarios)
                .NotEmpty().WithMessage("Debe asignar al menos un horario.");

            RuleForEach(x => x.Horarios).ChildRules(h =>
            {
                h.RuleFor(x => x.DiaAsignado)
                    .Must(d => d != DayOfWeek.Saturday && d != DayOfWeek.Sunday)
                    .WithMessage("El guía no puede tener horarios en fin de semana.");

                h.RuleFor(x => x.HoraInicio)
                    .NotNull();

                h.RuleFor(x => x.HoraFin)
                    .NotNull()
                    .Must((dto, horaFin) => horaFin > dto.HoraInicio)
                    .WithMessage("La hora de fin debe ser posterior a la hora de inicio.");
            });
        }
    }
}
