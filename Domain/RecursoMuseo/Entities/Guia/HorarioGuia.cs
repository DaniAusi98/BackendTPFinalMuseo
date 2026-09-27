using Core.Domain.Entities;
using Domain.Common.Exceptions;
using Domain.RecursoMuseo.ValueObjets;

namespace Domain.RecursoMuseo.Entities.Guia
{
    public class HorarioGuia : DomainEntity
    {
        // Navigation back to Guia
        public string GuiaId { get; private set; }
        public Guia Guia { get; private set; } = default!;

        // Días de la semana en los que aplica este horario
        public DiaLaboral DiaAsignado { get; private set; } 
        public TimeOnly HoraInicio { get; private set; }
        public TimeOnly HoraFin { get; private set; }

        public HorarioGuia()
        {
            // EF
        }

        public HorarioGuia(
            DiaLaboral dia,
            TimeOnly inicio,
            TimeOnly fin)
        {
            Id = Guid.NewGuid().ToString();




            if (fin <= inicio)
            {
                throw new DomainException(
                    "La hora de fin debe ser posterior a la hora de inicio.");
            }

            DiaAsignado = dia;
            HoraInicio = inicio;
            HoraFin = fin;
        }
        
    }
}
