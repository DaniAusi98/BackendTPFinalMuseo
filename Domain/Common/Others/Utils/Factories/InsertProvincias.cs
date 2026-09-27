using Domain.Common.Entities;

namespace Domain.Common.Others.Utils.Factories
{
    public static class InsertProvincias
    {
        public static List<Provincia> CrearProvincias()
        {
            var provincias = new List<Provincia>
            {
                new Provincia("02", "Ciudad Autónoma de Buenos Aires"),
                new Provincia("58", "Neuquén"),
                new Provincia("74", "San Luis"),
                new Provincia("82", "Santa Fe"),
                new Provincia("46", "La Rioja"),
                new Provincia("10", "Catamarca"),
                new Provincia("90", "Tucumán"),
                new Provincia("22", "Chaco"),
                new Provincia("34", "Formosa"),
                new Provincia("78", "Santa Cruz"),
                new Provincia("26", "Chubut"),
                new Provincia("50", "Mendoza"),
                new Provincia("30", "Entre Ríos"),
                new Provincia("70", "San Juan"),
                new Provincia("38", "Jujuy"),
                new Provincia("86", "Santiago del Estero"),
                new Provincia("62", "Río Negro"),
                new Provincia("18", "Corrientes"),
                new Provincia("54", "Misiones"),
                new Provincia("66", "Salta"),
                new Provincia("14", "Córdoba"),
                new Provincia("06", "Buenos Aires"),
                new Provincia("42", "La Pampa"),
                new Provincia("94", "Tierra del Fuego, Antártida e Islas del Atlántico Sur")
            };

            return provincias;
        }
    }
}