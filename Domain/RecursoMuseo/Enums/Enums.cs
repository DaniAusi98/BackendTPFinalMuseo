namespace Domain.RecursoMuseo.Enums
{
    public class Enums
    {
        /// <summary>
        /// Ejemplo de enumeracion Dummy
        /// </summary>
        public enum TipoSala
        {
            ExposicionPermanente,
            ExposicionTemporal,
            Hall,
            Biblioteca,
            Auditorio,
            AulaEducativa,
            ReservaPatrimonial,
            Documentacion,
            Multifuncion,
            ExposicionFotografica
        }
        public enum EstadoSala
        {
            Activa,
            Inactiva
        }

        public enum DatabaseType
        {
            MYSQL,
            MARIADB,
            SQLSERVER,
            MONGODB
        }

        public enum UbicacionSala
        {
            PlantaBaja,
            PrimerPiso,
            SegundoPiso
        }
        public enum TipoRecurso
        {
            Mobiliario,
            Tecnologico,
            Audiovisual
        }

        public enum EstadoRecurso
        {
            Disponible,
            Prestado,
            EnMantenimiento,
            NoDisponible
        }
    }
}
