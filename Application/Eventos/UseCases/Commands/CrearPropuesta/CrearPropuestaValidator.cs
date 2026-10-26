using FluentValidation;

namespace Application.Eventos.UseCases.Commands.CrearPropuesta
{
    public class CrearPropuestaCommandValidator
        : AbstractValidator<CrearPropuestaCommand>
    {
        public CrearPropuestaCommandValidator()
        {
            // Responsable
            RuleFor(x => x.NombreApellidoResponsable)
                .NotEmpty()
                .MaximumLength(200)
                .WithMessage("Debe indicar el nombre y apellido del responsable.");

            RuleFor(x => x.TelefonoResponsable)
                .NotEmpty()
                .WithMessage("Debe indicar el teléfono del responsable.");

            RuleFor(x => x.EmailResponsable)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Debe indicar un email válido.");

            // Propuesta
            RuleFor(x => x.TituloPropuesta)
                .NotEmpty()
                .MaximumLength(200)
                .WithMessage("Debe indicar el título de la propuesta.");

            RuleFor(x => x.TipoEvento)
                .IsInEnum()
                .WithMessage("El tipo de evento seleccionado no es válido.");

            // Fechas
            RuleFor(x => x.Fecha)
                .NotEmpty()
                .WithMessage("Debe indicar la fecha de inicio.");

            RuleFor(x => x.FechaHasta)
                .NotEmpty()
                .WithMessage("Debe indicar la fecha de fin.");

            RuleFor(x => x.FechaHasta)
                .GreaterThan(x => x.Fecha)
                .WithMessage("La fecha de fin debe ser posterior a la fecha de inicio.");

            // Salas
            RuleFor(x => x.SalasIds)
                .NotNull()
                .NotEmpty()
                .WithMessage("Debe seleccionar al menos una sala.");

            RuleForEach(x => x.SalasIds)
                .NotEmpty()
                .WithMessage("Los identificadores de las salas no pueden estar vacíos.");

            RuleFor(x => x.SalasIds)
                .Must(ids => ids == null || ids.Distinct().Count() == ids.Count)
                .WithMessage("No puede seleccionar la misma sala más de una vez.");

            RuleForEach(x => x.Recursos)
                .ChildRules(r =>
                {
                    r.RuleFor(x => x.RecursoId).NotEmpty();
                    r.RuleFor(x => x.CantidadAsignada).GreaterThan(0);
                });

            RuleFor(x => x.Recursos)
                .Must(rs => rs.Select(r => r.RecursoId).Distinct().Count() == rs.Count)
                .WithMessage("No debe repetir recursos.");

            // Recurrencia
            RuleFor(x => x.RRule)
                .MaximumLength(255)
                .When(x => !string.IsNullOrWhiteSpace(x.RRule))
                .WithMessage("La regla de recurrencia no puede superar los 255 caracteres.");
        }
    }
}