using FluentValidation;

namespace Application.MuseumResources.UseCases.Recurso.Queries.GetRecursoBy
{
    public class GetRecursoByValidator : AbstractValidator<GetRecursoByQuery>
    {
        public GetRecursoByValidator()
        {
            RuleFor(x => x.RecursoId)
                .GreaterThan(0);
        }
    }
}
