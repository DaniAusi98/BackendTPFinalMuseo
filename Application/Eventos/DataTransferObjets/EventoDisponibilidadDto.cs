namespace Application.Eventos.DataTransferObjets
{
    /// <summary>
    /// DTO que representa la disponibilidad de horas para un evento
    /// en un rango de fechas (típicamente un mes)
    /// </summary>
    public class EventoDisponibilidadDto
    {
        /// <summary>
        /// Período consultado (año-mes)
        /// </summary>
        public string Periodo { get; set; }

        /// <summary>
        /// Mapa de disponibilidad por hora
        /// Key: datetime en formato ISO (ej: "2024-01-15T09:00")
        /// Value: objeto con disponibilidad y razón del conflicto (si aplica)
        /// </summary>
        public List<HorarioDisponibleDto> HorariosDisponibles { get; set; } = new();
    }

    /// <summary>
    /// Disponibilidad de una hora específica para el evento
    /// </summary>
    public class HorarioDisponibleDto
    {
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public bool Disponible { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }
}
