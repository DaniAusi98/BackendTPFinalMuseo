using Domain.PersonalMuseo.Entities.UsuarioInterno;

namespace Infrastructure.Data.Seeders
{
    public static class AreaFactory
    {

        public static List<Area> CrearArasOficiales()
        {


            return new List<Area>
            {
                new Area
                (nombre :"Recepción",
                descripcion:"El Área de Recepción del Museo de Antropologías es el espacio de bienvenida y el primer contacto que se establece entre las personas que nos visitan y la institución. De ella dependerá el vínculo y la experiencia que vivirán las personas dentro del Museo."

                ),
                new Area(
                    nombre:"Educación",
                    descripcion:"El Área Educación del Museo de Antropologías elabora materiales y acciones educativas para y con las personas visitantes. Planifica, desarrolla y promueve proyectos específicos para la comunidad. Articula con instituciones de todos los niveles educativos, centros de día, organizaciones barriales y familias. Se encarga del contenido, diseño y producción de material didáctico de apoyo a las exhibiciones, tanto para desarrollar en la presencialidad como en formatos digitales."
                    ),
                new Area(nombre:"Comunicación",
                descripcion:"El Área trabaja y acompaña las acciones del Museo y sus equipos, elaborando piezas de comunicación que propicien el diálogo de saberes y el abordaje de temáticas diversas, vinculadas a las ciencias antropológicas, sus alcances e implicancia en problemáticas sociales contemporáneas.")

            };
        }
    }
}

