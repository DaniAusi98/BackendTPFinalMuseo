
using Domain.VisitasGrupales.ValueObjects;

namespace Domain.VisitasGrupales.Entities.GrupalGuiada;

public class ContextoDisponibilidadVisitaGuiada
{
    public ConfiguracionVisitasGrupalesGuiadas Configuracion { get; }

    public HorarioDisponible Horario { get; }

    public IReadOnlyCollection<VisitaGrupalGuiada> VisitasExistentes { get; }

    public int GuiasDisponibles { get; }

    public ContextoDisponibilidadVisitaGuiada(
        ConfiguracionVisitasGrupalesGuiadas configuracion,
        HorarioDisponible horario,
        IReadOnlyCollection<VisitaGrupalGuiada> visitasExistentes,
        int guiasDisponibles)
    {
        Configuracion = configuracion
            ?? throw new ArgumentNullException(nameof(configuracion));

        Horario = horario
            ?? throw new ArgumentNullException(nameof(horario));

        VisitasExistentes = visitasExistentes
            ?? throw new ArgumentNullException(nameof(visitasExistentes));

        if (guiasDisponibles < 0)
            throw new ArgumentOutOfRangeException(nameof(guiasDisponibles));

        GuiasDisponibles = guiasDisponibles;
    }
}
