
/*
namespace Domain.VisitasMuseo.DomainServices
{
    public class ServiciodeReprogramacionVisitaGuiada : IServicioReprogramacion
    {
        /// <summary>
        /// Reprograma una visita guiada de un turno a otro.
        /// </summary>
        /// <param name="visita">La visita a reprogramar.</param>
        /// <param name="turnoAnterior">El turno actual de la visita.</param>
        /// <param name="turnoNuevo">El turno al que se quiere mover la visita.</param>
        public void ReprogramarVisita(VisitaGrupalGuiada visita, Turno turnoAnterior, Turno turnoNuevo)
        {
            ArgumentNullException.ThrowIfNull(visita);
            ArgumentNullException.ThrowIfNull(turnoAnterior);
            ArgumentNullException.ThrowIfNull(turnoNuevo);

            // 1️⃣ Quitar la visita del turno actual
            turnoAnterior.QuitarReserva(visita);

            // 2️⃣ Cambiar el turnoId dentro de la visita
            visita.CambiarTurno(turnoNuevo.Id);

            // 3️⃣ Agregar la visita al nuevo turno
            turnoNuevo.AgregarReserva(visita);
        }
    }

}
*/
