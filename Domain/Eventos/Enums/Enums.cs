namespace Domain.Eventos.Enums
{
    public class Enums
    {
        public enum TipoEvento
        {
            Conferencia,
            Taller,
            Seminario,
            Exposicion,
            Charla,
            Paneles,
            Presentacion,
            Jornadas,
            DefensaTesis,
            CursoCapacitacion,
            Conversatorio,
            CineDebate,
            Otros,
            MuestraTemporal
        }
        public enum TipoEventoActividad
        {
            EventoActividadExterna,
            MuestraExposicionTemporal
        }





        public enum TipoPublico
        {
            General,
            InfantilFamiliar,
            Estudiantes,
            Investigadores,
            Docentes,
            AdultosMayores,
            OrganizacionesSociales,
            PublicoCerrado,
            Otros
        }


    }
}
