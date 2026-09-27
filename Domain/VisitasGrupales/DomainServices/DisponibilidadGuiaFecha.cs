using Domain.Common.ValueObjets;
using Domain.RecursoMuseo.Entities.Guia;

namespace Domain.VisitasGrupales.DomainServices
{
    public class DisponibilidadGuiaFecha
    {

        public static bool GuiaPuedeCubrirTurno(Guia guia, TimeSlot horario)
        {
            if (!guia.Activo)
                return false;
            DayOfWeek diaRequerido = horario.Inicio.DayOfWeek;

            TimeOnly horaInicioRequerida = TimeOnly.FromDateTime(horario.Inicio);
            TimeOnly horaFinRequerida = TimeOnly.FromDateTime(horario.Fin);

            bool cumpleHorario = guia.HorariosGuia.Any(h =>
                h.DiaAsignado.Dia == diaRequerido &&
                horaInicioRequerida >= h.HoraInicio &&
                horaFinRequerida <= h.HoraFin
            );

            if (!cumpleHorario)
                return false;

            bool tieneAusencia = guia.AusenciasProgramadas.Any(a =>
                horario.Inicio < a.FechaHasta &&
                horario.Fin > a.FechaDesde
            );

            return !tieneAusencia;
        }
    }
}
