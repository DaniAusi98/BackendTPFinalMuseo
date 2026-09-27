using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ActividadMuseo.UseCases.ActividadesMuseo.Commnads.BloqueoSala
{
    using FluentValidation;

    public class CrearBloqueoSalaCommandValidator
        : AbstractValidator<CrearBloqueoSalaCommand>
    {
        public CrearBloqueoSalaCommandValidator()
        {
            RuleFor(x => x.SalaId)
                .GreaterThan(0);

            RuleFor(x => x.FechaDesde)
                .NotEmpty();

            RuleFor(x => x.FechaHasta)
                .NotEmpty();

            RuleFor(x => x.FechaDesde)
                .LessThan(x => x.FechaHasta)
                .WithMessage("La fecha de inicio debe ser anterior a la fecha de fin.");

            RuleFor(x => x.Motivo)
                .IsInEnum();

            RuleFor(x => x.Observaciones)
                .MaximumLength(500);
        }
    }
}
