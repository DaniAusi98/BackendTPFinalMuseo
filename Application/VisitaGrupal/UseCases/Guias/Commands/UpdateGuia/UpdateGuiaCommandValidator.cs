using Application.VisitaGrupal.Repositories;
using FluentValidation;

namespace Application.VisitaGrupal.UseCases.Guias.Commands.UpdateGuia
{
    public class UpdateGuiaCommandValidator : AbstractValidator<UpdateGuiaCommand>
    {
        public UpdateGuiaCommandValidator(IRepositorioGuia repositorioGuia)
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .MustAsync(async (id, ct) =>
                    await repositorioGuia.FindOneAsync(id) != null)
                .WithMessage("El guía no existe.");

            RuleFor(x => x.Nombre)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Apellido)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(200);

            RuleFor(x => x.Telefono)
                .NotEmpty()
                .MaximumLength(30);

            RuleFor(x => x.Horarios)
                .NotEmpty()
                .WithMessage("Debe asignar al menos un horario.");

            RuleForEach(x => x.Horarios).ChildRules(h =>
            {
                h.RuleFor(x => x.DiaAsignado)
                    .Must(d => d != DayOfWeek.Saturday &&
                               d != DayOfWeek.Sunday)
                    .WithMessage("El guía no puede tener horarios en fin de semana.");

                h.RuleFor(x => x.HoraFin)
                    .Must((dto, horaFin) => horaFin > dto.HoraInicio)
                    .WithMessage("La hora de fin debe ser posterior a la hora de inicio.");
            });
        }
    }
}