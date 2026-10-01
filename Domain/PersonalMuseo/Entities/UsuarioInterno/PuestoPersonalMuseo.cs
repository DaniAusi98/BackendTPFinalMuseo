using Core.Domain.Entities;

namespace Domain.PersonalMuseo.Entities.UsuarioInterno
{
    public class PuestoPersonalMuseo : DomainEntity<string>
    {
        public string AreaPuestoId { get; private set; }
        public DateTime FechaDesde { get; private set; }
        public DateTime? FechaHasta { get; private set; }
        /* public ICollection<HorarioLaboral> HorariosLaborales { get; private set; }
             = new List<HorarioLaboral>();

         public ICollection<IndisponibilidadEmpleado> Indisponibilidades { get; private set; }
             = new List<IndisponibilidadEmpleado>();

         */
        protected PuestoPersonalMuseo()
        {

        }
        public PuestoPersonalMuseo(string areaPuestoId, DateTime fechaDesde, DateTime? fechaHasta)
        {
            Id = Guid.NewGuid().ToString();
            if (string.IsNullOrWhiteSpace(areaPuestoId))
                throw new ArgumentException("El Id del área-puesto no puede estar vacío.", nameof(areaPuestoId));
            if (fechaHasta.HasValue && fechaHasta.Value < fechaDesde)
                throw new ArgumentException("La fecha de finalización no puede ser anterior a la fecha de inicio.", nameof(fechaHasta));
            AreaPuestoId = areaPuestoId;
            FechaDesde = fechaDesde;
            FechaHasta = fechaHasta;
        }

        public void Modificar(
        string areaPuestoId,
        DateTime fechaDesde,
        DateTime? fechaHasta)
        {
            if (string.IsNullOrWhiteSpace(areaPuestoId))
                throw new ArgumentException("El Id del área-puesto no puede estar vacío.", nameof(areaPuestoId));
            if (fechaHasta.HasValue && fechaHasta.Value < fechaDesde)
                throw new ArgumentException("La fecha de finalización no puede ser anterior a la fecha de inicio.", nameof(fechaHasta));

            AreaPuestoId = areaPuestoId;
            FechaDesde = fechaDesde;
            FechaHasta = fechaHasta;
        }



    }
}


/* public void AgregarHorario(HorarioLaboral horario)
        {
            if (horario == null)
                throw new DomainException("El horario no puede ser nulo.");

            HorariosLaborales.Add(horario);
        }

        public void EliminarHorario(string horarioId)
        {
            var horario = HorariosLaborales.FirstOrDefault(x => x.Id == horarioId);

            if (horario == null)
                throw new DomainException("El horario no existe.");

            HorariosLaborales.Remove(horario);
        }

        public void AgregarIndisponibilidad(IndisponibilidadEmpleado indisponibilidad)
        {
            if (indisponibilidad == null)
                throw new DomainException("La indisponibilidad no puede ser nula.");

            Indisponibilidades.Add(indisponibilidad);
        }

        public void EliminarIndisponibilidad(string indisponibilidadId)
        {
            var indisponibilidad = Indisponibilidades
                .FirstOrDefault(x => x.Id == indisponibilidadId);

            if (indisponibilidad == null)
                throw new DomainException("La indisponibilidad no existe.");

            Indisponibilidades.Remove(indisponibilidad);
        }
*/