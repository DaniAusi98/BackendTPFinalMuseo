namespace Domain.Common.Others
{
    public static class SeSolapaEnTiempo
    {
        public static bool SolapaEnTiempo(ActividadMuseo.Entities.ActividadMuseo nuevaActividad, ActividadMuseo.Entities.ActividadMuseo actividadExistente)
        {
            if (nuevaActividad.Horario.SeSolapaCon(actividadExistente.Horario)){return true;}
            return false;
        }
    }
}
