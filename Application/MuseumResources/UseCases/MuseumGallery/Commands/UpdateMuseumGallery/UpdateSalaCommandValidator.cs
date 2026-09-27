using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using FluentValidation.Validators;
using Application.MuseumResources.Repositories;

namespace Application.MuseumResources.UseCases.MuseumGallery.Commands.UpdateMuseumGallery
{
    public class UpdateSalaCommandValidator:AbstractValidator<UpdateMuseumGalleryCommand>
    {
        public UpdateSalaCommandValidator(IRepositorioSala repositorioSala) {

            RuleFor(x => x.Id)
                .GreaterThan(0)
                .MustAsync(async (id, ct) => await repositorioSala.FindOneAsync(id) != null)
                .WithMessage("La sala no existe.");
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
            RuleFor(x => x.UbicacionSala)
                .IsInEnum();

        }


    }
}
/*using FluentValidation;

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

*/
