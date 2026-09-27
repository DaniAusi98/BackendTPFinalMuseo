using Domain.Common.Entities;
using Domain.Common.ValueObjets;
using Domain.VisitasGrupales.Entities;

namespace Domain.VisitasGrupales.Factories
{
    public static class ConfiguracionVisitasAutoguiadasFactory
    {
        /// <summary>
        /// Crea una configuración por defecto para visitas autoguiadas.
        /// 
        /// Configuración predeterminada:
        /// - Horario: 9:00 - 18:00
        /// - Duración de visita: 1 hora (tiempo que el grupo permanece en el museo)
        /// - Intervalo entre reservas: 30 minutos (nueva entrada cada 30 min)
        /// - Capacidad por grupo: 25 personas
        /// - Visitas simultáneas: 2 grupos
        /// - Capacidad total simultánea: 50 personas (25 × 2)
        /// - Días: Lunes a viernes
        /// 
        /// Ejemplo de slots generados:
        /// 09:00-10:00, 09:30-10:30, 10:00-11:00, 10:30-11:30, etc.
        /// </summary>
        public static ConfiguracionHorarioAutoguiada CrearConfiguracionPorDefecto(CalendarioMuseo calendario)
        {
            if (calendario == null)
                throw new ArgumentNullException(nameof(calendario));

            var horario = new Horario(
                inicio: new TimeOnly(9, 0),
                fin: new TimeOnly(18, 0)
            );

            var diasLaborales = new DiasLaboralesMuseo(
                new[]
                {
                    DayOfWeek.Monday,
                    DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Thursday,
                    DayOfWeek.Friday
                });

            var duracionVisita = TimeSpan.FromHours(1);      // Cada grupo permanece 1 hora
            var intervaloReservas = TimeSpan.FromMinutes(30); // Nueva entrada cada 30 min

            var configuracion = new ConfiguracionHorarioAutoguiada(
                capacidadMaximaPorGrupo: 25,
                diasDisponibles: diasLaborales,
                horarioDisponibleVisitaAutoguiadas: horario,
                calendario: calendario,
                duracionVisita: duracionVisita,
                intervaloReservas: intervaloReservas,
                visitasSimultaneasMaximas: 2,
                bloqueos: null // Sin bloqueos iniciales
            );

            // Validar que los días disponibles coincidan con los días de apertura del museo
            var diasDisponiblesDelMuseo = new DiasLaboralesMuseo(
                calendario.DiasApertura.Dias.ToArray());

            configuracion.ActualizarDiasDisponibles(diasDisponiblesDelMuseo, calendario);

            return configuracion;
        }

        /// <summary>
        /// Crea una configuración personalizada para visitas autoguiadas.
        /// </summary>
        public static ConfiguracionHorarioAutoguiada CrearConfiguracionPersonalizada(
            int capacidadMaximaPorGrupo,
            DiasLaboralesMuseo diasDisponibles,
            Horario horario,
            CalendarioMuseo calendario,
            TimeSpan duracionVisita,
            TimeSpan intervaloReservas,
            int visitasSimultaneasMaximas)
        {
            if (calendario == null)
                throw new ArgumentNullException(nameof(calendario));

            return new ConfiguracionHorarioAutoguiada(
                capacidadMaximaPorGrupo: capacidadMaximaPorGrupo,
                diasDisponibles: diasDisponibles,
                horarioDisponibleVisitaAutoguiadas: horario,
                calendario: calendario,
                duracionVisita: duracionVisita,
                intervaloReservas: intervaloReservas,
                visitasSimultaneasMaximas: visitasSimultaneasMaximas,
                bloqueos: null
            );
        }

        /// <summary>
        /// Crea una configuración con slots sin solapamiento.
        /// (Intervalo = Duración, grupos no se solapan en el tiempo)
        /// 
        /// Ejemplo:
        /// - Duración: 1 hora
        /// - Intervalo: 1 hora
        /// Slots: 09:00-10:00, 10:00-11:00, 11:00-12:00
        /// </summary>
        public static ConfiguracionHorarioAutoguiada CrearConfiguracionSinSolapamiento(
            CalendarioMuseo calendario,
            TimeSpan duracionVisita,
            int capacidadMaximaPorGrupo = 25)
        {
            if (calendario == null)
                throw new ArgumentNullException(nameof(calendario));

            var horario = new Horario(
                inicio: new TimeOnly(9, 0),
                fin: new TimeOnly(18, 0)
            );

            var diasLaborales = new DiasLaboralesMuseo(
                calendario.DiasApertura.Dias.ToArray());

            return new ConfiguracionHorarioAutoguiada(
                capacidadMaximaPorGrupo: capacidadMaximaPorGrupo,
                diasDisponibles: diasLaborales,
                horarioDisponibleVisitaAutoguiadas: horario,
                calendario: calendario,
                duracionVisita: duracionVisita,
                intervaloReservas: duracionVisita, // Mismo valor = sin solapamiento
                visitasSimultaneasMaximas: 1,      // Solo 1 grupo a la vez
                bloqueos: null
            );
        }
    }
}