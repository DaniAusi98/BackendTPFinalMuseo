namespace Domain.VisitasGrupales.Entities.GrupalGuiada;

public class ResultadoReglaDisponibilidad
{
    public bool EsValida { get; }

    public string Motivo { get; }

    private ResultadoReglaDisponibilidad(
        bool esValida,
        string motivo)
    {
        EsValida = esValida;
        Motivo = motivo;
    }

    public static ResultadoReglaDisponibilidad Valida()
        => new(true, null);

    public static ResultadoReglaDisponibilidad Invalida(
        string motivo)
        => new(false, motivo);
}