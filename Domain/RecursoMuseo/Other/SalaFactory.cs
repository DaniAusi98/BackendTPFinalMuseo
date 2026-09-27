using System;
using System.Collections.Generic;
using Domain.RecursoMuseo.Entities;
using static Domain.RecursoMuseo.Enums.Enums;

namespace Domain.RecursoMuseo.Other
{
    public static class SalaFactory
    {
        /// <summary>
        /// Genera el listado completo y actualizado de salas oficiales basadas en el Museo de Antropología.
        /// </summary>
        public static List<Sala> CrearSalasOficialesMuseo()
        {
            return new List<Sala>
            {
                // ==================== PLANTA BAJA ====================
                new Sala(
                    nombre: "Hall de Ingreso y Espacio Cultural",
                    tipoSala: TipoSala.Multifuncion, // Usado para muestras temporales, presentaciones, proyecciones y conversatorios
                    capacidad: 100, // Aforo amplio para eventos de pie o sentados
                    ubicacion: UbicacionSala.PlantaBaja,
                    codigoSala: "PB-HAL-CUL"
                ),
                new Sala(
                    nombre: "Patrimonio Cultural",
                    tipoSala: TipoSala.ExposicionPermanente,
                    capacidad: 30,
                    ubicacion: UbicacionSala.PlantaBaja,
                    codigoSala: "PB-PAT-CUL"
                ),
                new Sala(
                    nombre: "Arqueología del Siglo XIX",
                    tipoSala: TipoSala.ExposicionPermanente,
                    capacidad: 35,
                    ubicacion: UbicacionSala.PlantaBaja,
                    codigoSala: "PB-ARQ-XIX"
                ),
                new Sala(
                    nombre: "Arqueología Andina",
                    tipoSala: TipoSala.ExposicionPermanente,
                    capacidad: 40,
                    ubicacion: UbicacionSala.PlantaBaja,
                    codigoSala: "PB-ARQ-AND"
                ),
                new Sala(
                    nombre: "Arqueología del Ambato",
                    tipoSala: TipoSala.ExposicionPermanente,
                    capacidad: 35,
                    ubicacion: UbicacionSala.PlantaBaja,
                    codigoSala: "PB-ARQ-AMB"
                ),
                new Sala(
                    nombre: "Arqueología Serrana (Casa Pozo)",
                    tipoSala: TipoSala.ExposicionPermanente,
                    capacidad: 25,
                    ubicacion: UbicacionSala.PlantaBaja,
                    codigoSala: "PB-SER-POZ"
                ),
                new Sala(
                    nombre: "Mensajes de Identidad",
                    tipoSala: TipoSala.ExposicionPermanente,
                    capacidad: 40,
                    ubicacion: UbicacionSala.PlantaBaja,
                    codigoSala: "PB-MEN-IDE"
                ),
                new Sala(
                    nombre: "Excavación",
                    tipoSala: TipoSala.ExposicionPermanente,
                    capacidad: 20,
                    ubicacion: UbicacionSala.PlantaBaja,
                    codigoSala: "PB-EXC-INT"
                ),

                // ==================== PRIMER PISO ====================
                new Sala(
                    nombre: "Identidades y Rituales Andinos",
                    tipoSala: TipoSala.ExposicionPermanente,
                    capacidad: 45,
                    ubicacion: UbicacionSala.PrimerPiso,
                    codigoSala: "P1-RIT-AND"
                ),
                new Sala(
                    nombre: "Antropología Social",
                    tipoSala: TipoSala.ExposicionPermanente,
                    capacidad: 35,
                    ubicacion: UbicacionSala.PrimerPiso,
                    codigoSala: "P1-ANT-SOC"
                ),
                new Sala(
                    nombre: "Muestra Huachichocana",
                    tipoSala: TipoSala.ExposicionPermanente,
                    capacidad: 30,
                    ubicacion: UbicacionSala.PrimerPiso,
                    codigoSala: "P1-COL-HUA"
                ),
                new Sala(
                    nombre: "Negro sobre Blanco",
                    tipoSala: TipoSala.ExposicionFotografica,
                    capacidad: 20,
                    ubicacion: UbicacionSala.PrimerPiso,
                    codigoSala: "P1-NEGR-BLA"
                )
            };
        }

        /// <summary>
        /// Crea una instancia individual parametrizada de una Sala pasando por las validaciones del dominio.
        /// </summary>
        public static Sala CrearSalaPersonalizada(
            string nombre,
            TipoSala tipoSala,
            int capacidad,
            UbicacionSala ubicacion,
            string codigoSala)
        {
            return new Sala(nombre, tipoSala, capacidad, ubicacion, codigoSala);
        }
    }
}
