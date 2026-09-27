using Core.Domain.Entities;

namespace Domain.VisitasGrupales.Entities.Notificaciones
{
    public class AvisoConfirmacionVisitaGrupal:DomainEntity<int>
    {
        public int IdVisitaGrupal { get; set; }
        public int VisitanteId { get; set; }
        public string TokenConfirmacion { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public bool Confirmada { get; set; } = false;
        public DateTime? FechaConfirmacion { get; set; } = null;
        public DateTime FechaCaducidad { get; set; } = DateTime.UtcNow;
        
        protected AvisoConfirmacionVisitaGrupal() { }
        public AvisoConfirmacionVisitaGrupal(int idVisitaGrupal, int visitanteId, string tokenConfirmacion, DateTime fechaCaducidad)
        {
            IdVisitaGrupal = idVisitaGrupal;
            VisitanteId = visitanteId;
            TokenConfirmacion = tokenConfirmacion;
            FechaCaducidad = fechaCaducidad;
        }
        public bool IsExpired()
        {
            return DateTime.UtcNow > FechaCaducidad;
        }


        
    }
}
