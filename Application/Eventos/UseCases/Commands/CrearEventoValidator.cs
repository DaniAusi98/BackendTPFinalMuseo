using FluentValidation;

namespace Application.Eventos.UseCases.Commands
{
    public class CrearEventoValidator : AbstractValidator<CrearEventoCommand>
    {
        public CrearEventoValidator()
        {
            RuleFor(x => x.NombreyApellidoSolicitante)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.TelefonoSolicitante)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.EmailSolicitante)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(200);

            RuleFor(x => x.Institucion)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.TituloEvento)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.DescripcionEvento)
                .MaximumLength(2000);

            RuleFor(x => x.FundamentacionEvento)
                .MaximumLength(2000);

            RuleFor(x => x.TipoEvento)
                .IsInEnum();

            RuleFor(x => x.TipoPublico)
                .NotEmpty()
                .Must(tp => tp.Distinct().Count() == tp.Count)
                .WithMessage("Debe indicar al menos un tipo de público sin repetir.");

            RuleForEach(x => x.TipoPublico)
                .IsInEnum();

            RuleForEach(x => x.SalasIds)
                .Must(id => !string.IsNullOrWhiteSpace(id))
                .WithMessage("Los ids de sala no pueden estar vacíos.");

            RuleFor(x => x.SalasIds)
                .Must(ids => ids.Distinct().Count() == ids.Count)
                .WithMessage("No debe repetir salas.");

            RuleForEach(x => x.Recursos)
                .ChildRules(r =>
                {
                    r.RuleFor(x => x.RecursoId).NotEmpty();
                    r.RuleFor(x => x.CantidadAsignada).GreaterThan(0);
                });

            RuleFor(x => x.Recursos)
                .Must(rs => rs.Select(r => r.RecursoId).Distinct().Count() == rs.Count)
                .WithMessage("No debe repetir recursos.");

            When(x => x.RRule is not null, () =>
            {
                RuleFor(x => x.RRule)
                    .NotEmpty()
                    .WithMessage("La regla debe ser valida");
            });

            RuleFor(x => x.ConcurrenciaEstimada)
                .GreaterThan(0)
                .WithMessage("La cantidad estimada de asistentes debe ser mayor a cero.");

            RuleFor(x => x.UrlImagenes)
                .Must(urls => urls == null || urls.All(url =>
                    !string.IsNullOrWhiteSpace(url) &&
                    (Uri.IsWellFormedUriString(url, UriKind.Absolute) ||
                     Uri.IsWellFormedUriString(url, UriKind.Relative))))
                .WithMessage("Las URLs de imágenes deben ser válidas (relativas o absolutas).");

            RuleFor(x => x.Inicio)
                .NotEmpty();

            RuleFor(x => x.Fin)
                .NotEmpty();

            RuleFor(x => x)
                .Must(x => x.Inicio < x.Fin)
                .WithMessage("La fecha de inicio debe ser anterior a la fecha de fin.");
        }
    }
}