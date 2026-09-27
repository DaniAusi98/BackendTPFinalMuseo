using Domain.VisitasGrupales.Entities.GrupalGuiada;
using Domain.VisitasGrupales.Services.Disponibilidad;

namespace Domain.VisitasGrupales.Entities.GrupalGuiada.ReglasDisponibilidad;

public class MotorDisponibilidadVisitasGuiadas
{
    private readonly IReadOnlyCollection<IReglaDisponibilidadVisitaGuiada> _reglas;

    public MotorDisponibilidadVisitasGuiadas(
        IEnumerable<IReglaDisponibilidadVisitaGuiada> reglas)
    {
        _reglas = reglas.ToList().AsReadOnly();
    }

    public ResultadoDisponibilidad Evaluar(
        ContextoDisponibilidadVisitaGuiada contexto)
    {
        foreach (var regla in _reglas)
        {
            var resultado = regla.Evaluar(contexto);

            if (!resultado.EsValida)
            {
                return ResultadoDisponibilidad.NoDisponible(
                    resultado.Motivo!);
            }
        }

        // ÚNICO LUGAR DONDE SE OBTIENEN LOS CUPOS
        int cuposDisponibles =
            contexto.Configuracion.CalcularCapacidadDisponible(
                contexto.GuiasDisponibles);

        return ResultadoDisponibilidad.DisponibleCon(
            cuposDisponibles);
    }
}