using Domain.RecursoMuseo.Entities.Guia;
using Domain.RecursoMuseo.ValueObjets;

namespace Domain.RecursoMuseo.Factories
{
    public static class GuiaFactory
    {
        public static List<Guia> CrearGuiasPorDefecto()
        {
            return new List<Guia>
            {
                CrearGuiaManana("Carlos Gómez", "1"),
                CrearGuiaManana("María Fernández", "2"),
                CrearGuiaTarde("Juan Pérez", "3"),
                CrearGuiaTarde("Laura Rodríguez", "4")
            };
        }

        private static Guia CrearGuiaManana(
            string nombre,
            string personalInternoId)
        {
            var horarios = CrearHorarios(
                new TimeOnly(9, 0),
                new TimeOnly(13, 0));

            return new Guia(
                nombre,
                personalInternoId,
                horarios);
        }

        private static Guia CrearGuiaTarde(
            string nombre,
            string personalInternoId)
        {
            var horarios = CrearHorarios(
                new TimeOnly(14, 0),
                new TimeOnly(18, 0));

            return new Guia(
                nombre,
                personalInternoId,
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
                    inicio,
                    fin),

                new HorarioGuia(
                    new DiaLaboral(DayOfWeek.Tuesday),
                    inicio,
                    fin),

                new HorarioGuia(
                    new DiaLaboral(DayOfWeek.Wednesday),
                    inicio,
                    fin),

                new HorarioGuia(
                    new DiaLaboral(DayOfWeek.Thursday),
                    inicio,
                    fin),

                new HorarioGuia(
                    new DiaLaboral(DayOfWeek.Friday),
                    inicio,
                    fin)
            };
        }
    }
}