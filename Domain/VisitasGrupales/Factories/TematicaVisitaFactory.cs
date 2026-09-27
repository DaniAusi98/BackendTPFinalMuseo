using System;
using System.Collections.Generic;
using System.Linq;
using Domain.RecursoMuseo.Entities;
using Domain.VisitasGrupales.Entities;

namespace Domain.VisitasGrupales.Factories
{
    public static class TematicaVisitaFactory
    {
        /// <summary>
        /// Crea el catálogo de temáticas oficiales asociando dinámicamente las salas correspondientes del museo.
        /// </summary>
        public static List<TematicaVisita> CrearTematicasOficiales(List<Sala> salasDisponibles)
        {
            if (salasDisponibles == null || !salasDisponibles.Any())
                throw new ArgumentException("Se requieren salas válidas para construir las temáticas.", nameof(salasDisponibles));

            var tematicas = new List<TematicaVisita>();

            // 1. Arqueología (Serrana, Andina, Ambato, Siglo XIX)
            var salasArqueologia = salasDisponibles.Where(s =>
                s.CodigoSala == "PB-ARQ-XIX" ||
                s.CodigoSala == "PB-ARQ-AND" ||
                s.CodigoSala == "PB-ARQ-AMB" ||
                s.CodigoSala == "PB-SER-POZ").ToList();

            if (salasArqueologia.Any())
            {
                tematicas.Add(new TematicaVisita(
                    nombre: "Arqueología",
                    descripcion: "Recorrido por las piezas, excavaciones e historia material de las culturas prehispánicas y del siglo XIX.",
                    salas: salasArqueologia
                ));
            }

            // 2. Pueblos Originarios (Mensajes de Identidad + Identidades y Rituales Andinos)
            // Agregamos P1-RIT-AND a la búsqueda por código
            var salasPueblos = salasDisponibles.Where(s =>
                s.CodigoSala == "PB-MEN-IDE" ||
                s.CodigoSala == "P1-RIT-AND").ToList();

            if (salasPueblos.Any())
            {
                tematicas.Add(new TematicaVisita(
                    nombre: "Pueblos Originarios",
                    descripcion: "Muestra enfocada en la actualidad, cosmovisión, derechos, ritualidad y mensajes de identidad de las comunidades originarias locales y andinas.",
                    salas: salasPueblos
                ));
            }

            // 3. Racismo (Negro sobre Blanco)
            var salasRacismo = salasDisponibles.Where(s => s.CodigoSala == "P1-NEGR-BLA").ToList();
            if (salasRacismo.Any())
            {
                tematicas.Add(new TematicaVisita(
                    nombre: "Racismo",
                    descripcion: "Espacio de reflexión visual e histórica sobre los 200 años de racismo institucionalizado y visibilización afrodescendiente.",
                    salas: salasRacismo
                ));
            }

            // 4. Antropología Social
            var salasAntropologia = salasDisponibles.Where(s => s.CodigoSala == "P1-ANT-SOC").ToList();
            if (salasAntropologia.Any())
            {
                tematicas.Add(new TematicaVisita(
                    nombre: "Antropología Social",
                    descripcion: "Análisis de las problemáticas sociales contemporáneas, memoria colectiva y prácticas culturales urbanas y rurales.",
                    salas: salasAntropologia
                ));
            }

            // 5. Muestra Temporaria (Hall de Ingreso y Espacio Cultural)
            var salasTemporarias = salasDisponibles.Where(s => s.CodigoSala == "PB-HAL-CUL").ToList();
            if (salasTemporarias.Any())
            {
                tematicas.Add(new TematicaVisita(
                    nombre: "Muestra Temporaria",
                    descripcion: "Exhibiciones itinerantes, presentaciones de libros, conversatorios y proyecciones culturales del momento.",
                    salas: salasTemporarias
                ));
            }

            

            return tematicas;
        }
    }
}
