using FluentValidation;
using Application.MuseumResources.Repositories;

namespace Application.MuseumResources.UseCases.Recurso.Commands.UpdateRecurso
{
    public class UpdateRecursoCommandValidator : AbstractValidator<UpdateRecursoCommand>
    {
        public UpdateRecursoCommandValidator(IRepositorioRecurso repositorioRecurso)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .MustAsync(async (id, ct) => await repositorioRecurso.FindOneAsync(id) != null)
                .WithMessage("El recurso no existe.");

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
