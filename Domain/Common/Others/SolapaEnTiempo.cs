namespace Domain.Common.Others
{
    public static class SeSolapaEnTiempo
    {
        public static bool SolapaEnTiempo(ActividadMuseo.Entities.ActividadBase nuevaActividad, ActividadMuseo.Entities.ActividadBase actividadExistente)
        {
            if (nuevaActividad.Horario.SeSolapaCon(actividadExistente.Horario)){return true;}
            return false;
        }
    }
}
