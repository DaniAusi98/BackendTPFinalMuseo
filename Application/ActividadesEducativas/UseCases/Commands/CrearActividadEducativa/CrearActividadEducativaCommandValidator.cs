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
            RuleFor(x => x.Inicio)
            .NotEmpty();

            RuleFor(x => x.Fin)
                .NotEmpty();

            RuleFor(x => x)
                .Must(x => x.Inicio < x.Fin)
                .WithMessage("La fecha de inicio debe ser anterior a la fecha de fin.");
            RuleFor(x => x.CantidadAsistentes)
                .GreaterThan(0)
                .WithMessage("La cantidad estimada de asistentes debe ser mayor a cero.");
            RuleFor(x => x.SalasIds)
                .Must(ids => ids.Distinct().Count() == ids.Count)
                .WithMessage("No debe repetir salas.");
            RuleFor(x => x.UrlImagenes)
                .Must(urls => urls == null || urls.All(url =>
                    !string.IsNullOrWhiteSpace(url) &&
                    (Uri.IsWellFormedUriString(url, UriKind.Absolute) ||
                     Uri.IsWellFormedUriString(url, UriKind.Relative))))
                .WithMessage("Las URLs de imágenes deben ser válidas (relativas o absolutas).");
            RuleForEach(x => x.Recursos)
                .ChildRules(r =>
                {
                    r.RuleFor(x => x.RecursoId).NotEmpty();
                    r.RuleFor(x => x.CantidadAsignada).GreaterThan(0);
                });

            RuleFor(x => x.Recursos)
                .Must(rs => rs.Select(r => r.RecursoId).Distinct().Count() == rs.Count)
                .WithMessage("No debe repetir recursos.");
        }
    }
}