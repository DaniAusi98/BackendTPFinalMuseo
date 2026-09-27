using Core.Application;
using static Domain.VisitasGrupales.Enums.Enums;

namespace Application.VisitaGrupal.DomainEvents
{
    internal sealed class VisitaGuiadaCreated : DomainEvent
    {
        public string VisitaId { get; set; }
        public string UsuarioVisitanteId { get; set; }
        public string NombreInstitucion { get; set; }
        public NivelEducativo? NivelEducativo { get; set; }
        public string? AnioGrado { get; set; }
        public int CantidadPersonas { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public DateTime FechaCreacion { get; set; }

    }
}
