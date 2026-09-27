using Application.ActividadMuseo.UseCases.ActividadesMuseo.Commnads.DiaCierreMuseo;
using Application.Common.UseCases.ActividadesMuseo.Commnads.DiaCierreMuseo;
using FluentValidation;

namespace Application.ActividadMuseo.UseCases.ActividadesMuseo.Commnads.DiaCierreMuseo
{
    public class CrearDiaCierreMuseoCommandValidator
    : AbstractValidator<CrearDiaCierreMuseoCommand>
    {
        public CrearDiaCierreMuseoCommandValidator()
        {
            RuleFor(x => x.Fecha)
                .NotEmpty();

            RuleFor(x => x.FechaHasta)
                .NotEmpty();

            RuleFor(x => x.Fecha)
                .LessThan(x => x.FechaHasta)
                .WithMessage("La fecha de inicio debe ser anterior a la fecha de fin.");

            RuleFor(x => x.Motivo)
                .IsInEnum();

            RuleFor(x => x.Observaciones)
                .MaximumLength(500);
        }
    }
}
