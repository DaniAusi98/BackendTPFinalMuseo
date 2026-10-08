namespace Application.VisitaGrupal.UseCases.Guias.Commands.CrearNoDisponibilidadGuia
{
    using FluentValidation;

    namespace Application.VisitaGrupal.UseCases.Guias.Commands.CrearNoDisponibilidadGuia
    {
        public class CrearNoDisponibilidadGuiaCommandValidator
            : AbstractValidator<CrearNoDisponibilidadGuiaCommand>
        {
            public CrearNoDisponibilidadGuiaCommandValidator()
            {
                RuleFor(x => x.GuiaId)
                .NotEmpty();

                RuleFor(x => x.FechaHasta)
                    .GreaterThanOrEqualTo(x => x.FechaDesde)
                    .WithMessage("La fecha hasta debe ser igual o posterior a la fecha desde.");

                RuleFor(x => x.Motivo)
                    .NotEmpty()
                    .MaximumLength(500);
            }
        }
    }
}
