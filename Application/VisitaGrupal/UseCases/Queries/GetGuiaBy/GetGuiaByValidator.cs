using FluentValidation;

namespace Application.VisitaGrupal.UseCases.Queries.GetGuiaBy
{
    public class GetGuiaByValidator : AbstractValidator<GetGuiaByQuery>
    {
        public GetGuiaByValidator()
        {
            RuleFor(x => x.GuiaId)
                .GreaterThan(0);
        }
    }
}
