using Domain.ActividadesAreaEducacion.Entities;

namespace Domain.ActividadesAreaEducacion.Other
{
    public static class ProyectoAreaEducacionFactory
    {
        public static List<ProyectosAreaEducacion> CrearProyectosOficialesMuseo()
        {
            return new List<ProyectosAreaEducacion>
            {
                new ProyectosAreaEducacion(
                    nombre: "Proyecto de Educación Patrimonial",
                    fechaInicio: new DateTime(2025, 1, 1),
                    fechaFin: new DateTime(2025, 12, 31),
                    equipoTrabajoResponsable: new List<EquipoTrabajoResponsable>
                    {
                        new EquipoTrabajoResponsable(
                            nombreCompleto: "Nombre Responsable 1",
                            institucionPerteneciente: "Museo de Antropología"),

                        new EquipoTrabajoResponsable(
                            nombreCompleto: "Nombre Responsable 2",
                            institucionPerteneciente: "Universidad Nacional de Córdoba")
                    },
                    publico: "Instituciones educativas y público general"
                ),

                new ProyectosAreaEducacion(
                    nombre: "Proyecto de Educación y Comunidad",
                    fechaInicio: new DateTime(2025, 1, 1),
                    fechaFin: new DateTime(2025, 12, 31),
                    equipoTrabajoResponsable: new List<EquipoTrabajoResponsable>
                    {
                        new EquipoTrabajoResponsable(
                            nombreCompleto: "Nombre Responsable 3",
                            institucionPerteneciente: "Museo de Antropología"),

                        new EquipoTrabajoResponsable(
                            nombreCompleto: "Nombre Responsable 4",
                            institucionPerteneciente: "Universidad Nacional de Córdoba")
                    },
                    publico: "Comunidad educativa y organizaciones sociales"
                )
            };
        }

        public static ProyectosAreaEducacion CrearProyectoPersonalizado(
            string nombre,
            DateTime fechaInicio,
            DateTime fechaFin,
            List<EquipoTrabajoResponsable> equipoTrabajoResponsables,
            string publico)
        {
            return new ProyectosAreaEducacion(
                nombre,
                fechaInicio,
                fechaFin,
                equipoTrabajoResponsables,
                publico);
        }
    }
}