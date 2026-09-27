using FluentValidation;

namespace Application.MuseumResources.UseCases.MuseumGallery.Commands.CreateMuseumGallery
{
    public class CreateSalaCommandValidator:AbstractValidator<CreateMuseumGalleryCommand>
    {
        public CreateSalaCommandValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty()
                .MaximumLength(100);
            RuleFor(x => x.CodigoSala)
                .NotEmpty()
                .MaximumLength(50);
            RuleFor(x => x.Capacidad)
                .GreaterThan(0);
            RuleFor(x => x.TipoSala)
                .IsInEnum();
            RuleFor(x => x.Ubicacion)
                .IsInEnum();
        }
    }
}

