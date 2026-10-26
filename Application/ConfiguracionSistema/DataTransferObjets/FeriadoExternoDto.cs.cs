namespace Application.ConfiguracionSistema.DataTransferObjets
{
    public sealed record FeriadoExternoDto(
        DateOnly Fecha,
        string Nombre,
        AmbitoFeriado Ambito,
        TipoFeriado Tipo
    );

    public enum AmbitoFeriado
    {
        Nacional,
        Provincial
    }

    public enum TipoFeriado
    {
        Inamovible,
        Trasladable,
        NoLaborable,
        Otro
    }
}