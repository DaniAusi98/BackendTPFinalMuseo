using Core.Domain.Entities;
using Domain.Common.ValueObjets;
using static Domain.ActividadMuseo.Enums.Enums;
using static Domain.VisitasGrupales.Entities.GrupalGuiada.Enums.Enums;

namespace Domain.VisitasGrupales.Entities.GrupalGuiada
{
    public class VisitaReprogramada : DomainEntity<string>
    {
        public string VisitaReprogramadaId { get; private set; }
        public string NuevaVisitaId { get; private set; }
        public TimeSlot HorarioReprogrado { get; private set; }
        public string Institucion { get; private set; }
        public NivelEducativo? NivelEducativo { get; private set; }
        public string? AnioGrado { get; private set; }
        public string PaisInstitucion { get; private set; }
        public string ProvinciaInstitucion { get; private set; }
        public string LocalidadInstitucion { get; private set; }
        public EstadoActividad Estado { get; private set; }
        public int? CantidadPersonas { get; private set; }
        public TimeSlot NuevoHorario { get; private set; }

        protected VisitaReprogramada()
        {

        }
        public VisitaReprogramada(string visitaReprogramadaId, string nuevaVisitaId, TimeSlot horarioReprogramado, string institucion, NivelEducativo? nivelEducativo, string? anioGrado, string paisInstitucion, string provinciaInstitucion, string localidadInstitucion, EstadoActividad estado, int? cantidadPersonas, TimeSlot nuevoHorario)
        {
            if (string.IsNullOrWhiteSpace(visitaReprogramadaId))
                throw new ArgumentException("El ID de la visita reprogramada no puede ser nulo o vacío.", nameof(visitaReprogramadaId));
            if (string.IsNullOrWhiteSpace(nuevaVisitaId))
                throw new ArgumentException("El ID de la nueva visita no puede ser nulo o vacío.", nameof(nuevaVisitaId));
            if (horarioReprogramado is null)
                throw new ArgumentNullException(nameof(horarioReprogramado), "El horario reprogramado no puede ser nulo.");
            if (string.IsNullOrWhiteSpace(institucion))
                throw new ArgumentException("La institución no puede ser nula o vacía.", nameof(institucion));
            if (string.IsNullOrWhiteSpace(paisInstitucion))
                throw new ArgumentException("El país de la institución no puede ser nulo o vacío.", nameof(paisInstitucion));
            if (string.IsNullOrWhiteSpace(provinciaInstitucion))
                throw new ArgumentException("La provincia de la institución no puede ser nula o vacía.", nameof(provinciaInstitucion));
            if (string.IsNullOrWhiteSpace(localidadInstitucion))
                throw new ArgumentException("La localidad de la institución no puede ser nula o vacía.", nameof(localidadInstitucion));
            if (nuevoHorario is null)
                throw new ArgumentNullException(nameof(nuevoHorario), "El nuevo horario no puede ser nulo.");
            if (cantidadPersonas is not null && cantidadPersonas <= 0)

                throw new ArgumentException("La cantidad de personas debe ser mayor a cero.", nameof(cantidadPersonas));
            Id = Guid.NewGuid().ToString();

            VisitaReprogramadaId = visitaReprogramadaId;
            NuevaVisitaId = nuevaVisitaId;
            HorarioReprogrado = horarioReprogramado;
            Institucion = institucion;
            NivelEducativo = nivelEducativo;
            AnioGrado = anioGrado;
            PaisInstitucion = paisInstitucion;
            ProvinciaInstitucion = provinciaInstitucion;
            LocalidadInstitucion = localidadInstitucion;
            Estado = estado;
            CantidadPersonas = cantidadPersonas;
            NuevoHorario = nuevoHorario;
        }

    }
}
