namespace Domain.VisitasGrupales.Services.Disponibilidad;

public class ResultadoDisponibilidad
{
    public bool Disponible { get; }

    public int CuposDisponibles { get; }

    public string? Motivo { get; }

    private ResultadoDisponibilidad(
        bool disponible,
        int cuposDisponibles,
        string? motivo)
    {
        Disponible = disponible;
        CuposDisponibles = cuposDisponibles;
        Motivo = motivo;
    }

    public static ResultadoDisponibilidad DisponibleCon(
        int cuposDisponibles)
        => new(
            true,
            cuposDisponibles,
            null);

    public static ResultadoDisponibilidad NoDisponible(
        string motivo)
        => new(
            false,
            0,
            motivo);
}