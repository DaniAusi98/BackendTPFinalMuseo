using Domain.VisitasGrupales.ValueObjects;

namespace Domain.VisitasGrupales.Entities.GrupalGuiada;

public class ContextoDisponibilidadVisitaGuiada
{
    public ConfiguracionVisitasGrupalesGuiadas Configuracion { get; }

    public TurnoVisitaGuiada Turno { get; }

    public IReadOnlyCollection<VisitaGrupalGuiada> VisitasExistentes { get; }

    public int GuiasDisponibles { get; }

    public ContextoDisponibilidadVisitaGuiada(
        ConfiguracionVisitasGrupalesGuiadas configuracion,
        TurnoVisitaGuiada turno,
        IReadOnlyCollection<VisitaGrupalGuiada> visitasExistentes,
        int guiasDisponibles)
    {
        Configuracion = configuracion;
        Turno = turno;
        VisitasExistentes = visitasExistentes;
        GuiasDisponibles = guiasDisponibles;
    }
}