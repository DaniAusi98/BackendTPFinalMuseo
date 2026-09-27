namespace Domain.VisitasGrupales.Constants
{
    internal static class DomainConstants
    {
        public const string NOTNULL_OR_EMPTY = "El valor no puede ser nulo o vacio.";
        public const string NOTNULL_OR_EMPTY_OR_WHITESPACE = "El valor no puede ser nulo, vacio o contener solo espacios en blanco.";
        public const string GREATER_THAN_ZERO = "El valor debe ser mayor a cero.";
        public const string INICIO_NODEFECTO = "La fecha/hora de inicio no puede ser valor por defecto.";
        public const string FECHA_OBLIGATORIA = "La fecha/hora de fin es obligatoria.";
        public const string FIN_NODEFECTO = "La fecha/hora de fin no puede ser valor por defecto.";
        public const string INICIOMENOR_FIN = "La fecha/hora de inicio debe ser anterior a la fecha/hora de fin.";
        public const string DURACION_MAYOR_TREINTA_MIN = "La duración mínima debe ser de al menos un minuto.";
        public const string NOFECHAS_PASADAS = "La fecha/hora de inicio no puede ser en el pasado.";
        public const string RECURSO_VALIDO = "Debe asignar un recurso válido.";
        public const string ACTIVIDAD_VALIDA = "Debe asociarse a una actividad.";
        public const string CATEGORIA_RECURSO_VALIDA = "La categoría debe ser un valor válido.";
        public const string TIPO_SALA_VALIDA = "El tipo de sala debe ser un valor válido.";
        public const string UBICACION_VALIDA = "El tipo de ubicacion debe ser un valor válido.";
        public const string NO_RECURSOS_REPETIDOS = "No puede haber recursos repetidos en la actividad.";
        public const string NO_HORARIOS_SOLAPADOS = "Los horarios no pueden solaparse";
        public const string SALA_VALIDA = "Si se especifica, la sala debe ser válida.";
        public const string GUIA_ID_INVALIDO = "El Id de guía debe ser un valor válido.";
        public const string TURNO_ID_INVALIDO = "El Id de turno debe ser un valor válido.";
        public const string TURNO_ESTADO = "El turno debe tener al menos un guía asignado.";
        public const string TURNO_GUIA_ASIGNADO = "El turno debe tener al menos un guía asignado.";
        public const string TURNO_HORARIO = "El turno debe tener un horario asignado.";
        private const int MaxReservas = 2;




    }
}
