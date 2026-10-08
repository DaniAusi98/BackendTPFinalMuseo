using Domain.PersonalMuseo.Entities.UsuarioInterno;

namespace Infrastructure.Data.Seeders
{
    public static class PuestoFactory
    {
        public static List<Puesto> CrearPuestosOficiales()
        {
            return new List<Puesto>
            {
                new Puesto(
                    nombre: "Recepcionista",
                    descripcion: "Responsable de recibir y orientar a las personas visitantes."
                ),

                new Puesto(
                    nombre: "Educador",
                    descripcion: "Responsable de desarrollar actividades educativas."
                ),

                new Puesto(
                    nombre: "Comunicador",
                    descripcion: "Responsable de la comunicación institucional y difusión del museo."
                ),

                new Puesto(
                    nombre: "Guía",
                    descripcion: "Responsable de realizar visitas guiadas."
                )
            };
        }
    }
}