using Domain.Common.ValueObjets;
using Domain.RecursoMuseo.ValueObjets;
using Domain.VisitasGrupales.Entities.GrupalGuiada.Guia;

namespace Domain.RecursoMuseo.Factories
{
    public static class GuiaFactory
    {
        public static List<Guia> CrearGuiasPorDefecto()
        {
            return new List<Guia>
            {
                CrearGuiaManana(
                    "Carlos", "Gómez",
                    "carlos@ museo.com", "3511111111"),

                CrearGuiaManana(
                    "María", "Fernández",
                    "maria@museo.com", "3512222222"),

                CrearGuiaTarde(
                    "Juan", "Pérez",
                    "juan@museo.com", "3513333333"),

                CrearGuiaTarde(
                    "Laura", "Rodríguez",
                    "laura@museo.com", "3514444444")
            };
        }

        private static Guia CrearGuiaManana(
            string nombre,
            string apellido,
            string email,
            string telefono)
        {
            var horarios = CrearHorarios(
                new TimeOnly(9, 0),
                new TimeOnly(13, 0));

            return new Guia(
                nombre,
                apellido,
                new Email(email),
                new Telefono(telefono),
                horarios);
        }

        private static Guia CrearGuiaTarde(
            string nombre,
            string apellido,
            string email,
            string telefono)
        {
            var horarios = CrearHorarios(
                new TimeOnly(14, 0),
                new TimeOnly(18, 0));

            return new Guia(
                nombre,
                apellido,
                new Email(email),
                new Telefono(telefono),
                horarios);
        }

        private static List<HorarioGuia> CrearHorarios(
            TimeOnly inicio,
            TimeOnly fin)
        {
            return new List<HorarioGuia>
            {
                new HorarioGuia(
                    new DiaLaboral(DayOfWeek.Monday),
                    inicio, fin),

                new HorarioGuia(
                    new DiaLaboral(DayOfWeek.Tuesday),
                    inicio, fin),

                new HorarioGuia(
                    new DiaLaboral(DayOfWeek.Wednesday),
                    inicio, fin),

                new HorarioGuia(
                    new DiaLaboral(DayOfWeek.Thursday),
                    inicio, fin),

                new HorarioGuia(
                    new DiaLaboral(DayOfWeek.Friday),
                    inicio, fin)
            };
        }
    }
}