using FluentValidation;

namespace Application.MuseumResources.UseCases.Recurso.Commands.CreateRecurso
{
    public class CreateRecursoCommandValidator : AbstractValidator<CreateRecursoCommand>
    {
        public CreateRecursoCommandValidator()
        {
            RuleFor(x => x.NombreRecurso)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.TipoRecurso)
                .IsInEnum();

            RuleFor(x => x.Descripcion)
                .NotEmpty()
                .MaximumLength(1000);

            RuleFor(x => x.CantidadTotal)
                .GreaterThan(0);
        }
    }
}
