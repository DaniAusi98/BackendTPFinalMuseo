using Application.VisitaGrupal.UseCases.Comands.CrearVisitaGuiada;

using FluentValidation;

public class CrearVisitaGuiadaCommandValidator
    : AbstractValidator<CrearVisitaGuiadaCommand>
{
    public CrearVisitaGuiadaCommandValidator()
    {
        // Usuario
        RuleFor(x => x.UsuarioVisitanteId)
            .NotEmpty()
            .WithMessage("Debe indicar el usuario visitante.");

        // Institución
        RuleFor(x => x.Institucion)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.EmailInstitucion)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.TelefonoInstitucion)
            .NotEmpty();

        RuleFor(x => x.PaisInstitucion)
           .NotEmpty()
           .MaximumLength(100);

        RuleFor(x => x.ProvinciaInstitucion)
            .NotEmpty()
            .MaximumLength(100);

       ;

        RuleFor(x => x.LocalidadInstitucion)
            .NotEmpty()
            .MaximumLength(100);

        // Cantidad de personas
        RuleFor(x => x.CantidadPersonas)
            .GreaterThan(0);

        // Nivel educativo / Año
        RuleFor(x => x.AnioGrado)
            .NotNull()
            .When(x => x.NivelEducativo.HasValue)
            .WithMessage("Debe indicar el año o grado.");

        RuleFor(x => x)
            .Must(x => x.NivelEducativo.HasValue || string.IsNullOrEmpty(x.AnioGrado))
            .WithMessage("No puede indicar año/grado sin nivel educativo.");

        RuleFor(x => x.Inicio)
            .NotEmpty();
        
        RuleFor(x => x.Fin)
            .NotEmpty();

        RuleFor(x => x.Inicio)
            .LessThan(x => x.Fin)
            .WithMessage("La fecha de inicio debe ser anterior a la fecha de fin.");
        // Observaciones
        RuleFor(x => x.Observaciones)
            .MaximumLength(1000);

        RuleFor(x => x.DiversidadFuncionalDescripcion)
            .MaximumLength(500);

        RuleFor(x => x.MotivoRelacionVisita)
            .MaximumLength(500);
    }
}
