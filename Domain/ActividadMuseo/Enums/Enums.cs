namespace Domain.ActividadMuseo.Enums
{
    /// <summary>
    /// Las enumeraciones deben ir definidas aqui
    /// </summary>

    public static class Enums
    {
        /// <summary>
        /// Ejemplo de enumeracion Dummy
        /// </summary>
        public enum DummyValues
        {
            value1,
            value2,
            value3,
        }

        public enum DatabaseType
        {
            MYSQL,
            MARIADB,
            SQLSERVER,
            MONGODB
        }

        public enum TipoActividad
        {
            VisitaGrupalGuiada,
            VisitaGrupalAutoguiada,
            Evento,
            ActividadExterna,
            ActividadEducativa,     
            MuestraExposicionTemporal,
            ActividadEspecial
        }
        public enum EstadoActividad
        {

            Activa,
            Reprogramada,
            Cancelada
        }

        public enum CategoriaActividad
        {
            VisitaGrupal,
            EventoActividadExterna,
            ActividadEducativa,
            EventosEspeciales

        }
        public enum Frecuencia
        {
            Diaria,
            Semanal,
            Mensual,
            RangoFechas,
            DiasHabiles
        }

    }
}
