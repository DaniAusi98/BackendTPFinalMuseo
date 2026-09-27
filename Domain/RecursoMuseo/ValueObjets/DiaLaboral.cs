using Domain.Common.Exceptions;
using Domain.Common.ValueObjets;

namespace Domain.RecursoMuseo.ValueObjets;

public class DiaLaboral: ValueObject
{
    public DayOfWeek Dia { get; }

    public DiaLaboral(DayOfWeek dia)
    {
    
        if (dia is DayOfWeek.Saturday or DayOfWeek.Sunday)
            throw new DomainException("Los guías no trabajan fines de semana.");

        Dia = dia;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Dia;
    }
}
