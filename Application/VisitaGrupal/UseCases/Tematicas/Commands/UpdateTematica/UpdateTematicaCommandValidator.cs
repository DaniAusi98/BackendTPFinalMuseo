using FluentValidation;
using Application.MuseumResources.Repositories;
using Application.VisitaGrupal.Repositories;

namespace Application.VisitaGrupal.UseCases.Tematicas.Commands.UpdateTematica
{
    public class UpdateTematicaCommandValidator : AbstractValidator<UpdateTematicaCommand>
    {
        public UpdateTematicaCommandValidator(IRepositorioTematicas repositorioTematica, IRepositorioSala repositorioSala)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .MustAsync(async (id, ct) => await repositorioTematica.FindOneAsync(id) != null)
                .WithMessage("La temática no existe.");

            RuleFor(x => x.Nombre)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Descripcion)
                .MaximumLength(1000);

            RuleFor(x => x.SalaIds)
                .NotEmpty().WithMessage("Debe asignar al menos una sala.");

            RuleForEach(x => x.SalaIds)
                .GreaterThan(0)
                .MustAsync(async (id, ct) => await repositorioSala.FindOneAsync(id) != null)
                .WithMessage("La sala indicada no existe.");
        }
    }
}
