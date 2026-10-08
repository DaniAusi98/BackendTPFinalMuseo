using Domain.PersonalMuseo.Entities.UsuarioInterno;

namespace Infrastructure.Data.Seeders
{
    public static class AreaPuestoFactory
    {
        public static List<AreaPuesto> CrearAsignacionesOficiales(
            IReadOnlyCollection<Area> areas,
            IReadOnlyCollection<Puesto> puestos)
        {
            var recepcion = areas.First(x => x.Nombre == "Recepción");
            var educacion = areas.First(x => x.Nombre == "Educación");
            var comunicacion = areas.First(x => x.Nombre == "Comunicación");

            var recepcionista = puestos.First(x => x.Nombre == "Recepcionista");
            var educador = puestos.First(x => x.Nombre == "Educador");
            var comunicador = puestos.First(x => x.Nombre == "Comunicador");
            var guia = puestos.First(x => x.Nombre == "Guía");

            return new List<AreaPuesto>
            {
                new AreaPuesto(recepcion.Id, recepcionista.Id),
                new AreaPuesto(educacion.Id, educador.Id),
                new AreaPuesto(comunicacion.Id, comunicador.Id),
                new AreaPuesto(educacion.Id, guia.Id)
            };
        }
    }
}