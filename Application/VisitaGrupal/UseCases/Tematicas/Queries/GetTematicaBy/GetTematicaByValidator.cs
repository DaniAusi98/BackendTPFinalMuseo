using FluentValidation;

namespace Application.VisitaGrupal.UseCases.Tematicas.Queries.GetTematicaBy
{
    public class GetTematicaByValidator : AbstractValidator<GetTematicaByQuery>
    {
        public GetTematicaByValidator()
        {
            RuleFor(x => x.TematicaId)
                .GreaterThan(0);
        }
    }
}
