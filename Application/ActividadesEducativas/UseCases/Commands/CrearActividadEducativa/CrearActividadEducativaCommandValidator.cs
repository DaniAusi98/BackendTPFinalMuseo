using FluentValidation;

namespace Application.ActividadesEducativas.UseCases.Commands.CrearActividadEducativa
{
    public class CrearActividadEducativaCommandValidator
        : AbstractValidator<CrearActividadEducativaCommand>
    {
        public CrearActividadEducativaCommandValidator()
        {
            // Título
            RuleFor(x => x.Titulo)
                .NotEmpty()
                .MaximumLength(200)
                .WithMessage("El título es obligatorio y no puede superar los 200 caracteres.");

            // Tipo de actividad
            RuleFor(x => x.TipoActividadEducativa)
                .IsInEnum()
                .WithMessage("El tipo de actividad educativa no es válido.");

            // Proyecto vinculado
            RuleFor(x => x.ProyectoVinculadaId)
                .NotEmpty()
                .WithMessage("Debe indicar el proyecto vinculado.");

            // Institución
            RuleFor(x => x.InstitucionVinculada)
                .NotEmpty()
                .MaximumLength(200)
                .WithMessage("La institución vinculada es obligatoria y no puede superar los 200 caracteres.");

            // Público objetivo
            RuleFor(x => x.PublicoObjetivo)
                .NotEmpty()
                .MaximumLength(200)
                .WithMessage("El público objetivo es obligatorio y no puede superar los 200 caracteres.");

            // Observaciones
            RuleFor(x => x.Observaciones)
                .MaximumLength(1000)
                .WithMessage("Las observaciones no pueden superar los 1000 caracteres.");
        }
    }
}