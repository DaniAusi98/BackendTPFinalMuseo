
using Application.ActividadMuseo.Repositories;
using Application.Availability.Factories;
using Application.Availability.Models;
using Application.ConfiguracionSistema.Repositories;
using Application.MuseumResources.Repositories;
using Domain.Common.Entities;
using Domain.Common.ValueObjets;
using Domain.Eventos.Entities;
using Domain.Eventos.ValueObjets;
using static Domain.ActividadMuseo.Enums.Enums;

using ActividadMuseoEntity =
    Domain.ActividadMuseo.Entities.ActividadBase;

namespace Application.Availability.Producers
{
    public class DisponibilidadMuestrasExposicionesService(
        IRepositorioActividadMuseo repositorioActividadMuseo,
        IRepositorioCalendarioMuseo repositorioCalendario,
        IRepositorioSala repositorioSalaMuseo,
        AvailabilityEngine engine,
        ActivityAvailabilityFactory availabilityFactory)
    {
        private readonly IRepositorioActividadMuseo _repositorioActividadMuseo =
            repositorioActividadMuseo
            ?? throw new ArgumentNullException(nameof(repositorioActividadMuseo));

        private readonly IRepositorioCalendarioMuseo _repositorioCalendario =
            repositorioCalendario
            ?? throw new ArgumentNullException(nameof(repositorioCalendario));

        private readonly IRepositorioSala _repositorioSalaMuseo =
            repositorioSalaMuseo
            ?? throw new ArgumentNullException(nameof(repositorioSalaMuseo));

        private readonly AvailabilityEngine _engine = engine
            ?? throw new ArgumentNullException(nameof(engine));

        private readonly ActivityAvailabilityFactory _availabilityFactory =
            availabilityFactory
            ?? throw new ArgumentNullException(nameof(availabilityFactory));


        public async Task<bool> EstaDisponibleAsync(
            List<string> salasIds,
            TimeSlot horarioMuestra,
            LogisticaMuestraTemporal logisticaMuestraTemporal,
            string? muestraIdExcluir = null)
        {
            // 1. Validar parámetros.
            ArgumentNullException.ThrowIfNull(salasIds);
            ArgumentNullException.ThrowIfNull(horarioMuestra);
            ArgumentNullException.ThrowIfNull(logisticaMuestraTemporal);

            if (horarioMuestra.Inicio >= horarioMuestra.Fin)
                throw new ArgumentException(
                    "La fecha de inicio debe ser anterior a la fecha de fin.");

            if (salasIds.Count == 0)
                throw new ArgumentException(
                    "Debe seleccionar al menos una sala.",
                    nameof(salasIds));

            var desde = horarioMuestra.Inicio;
            var hasta = horarioMuestra.Fin;


            // 2. Obtener el calendario activo.
            var calendario = await _repositorioCalendario
                .ObtenerCalendarioActivoAsync()
                ?? throw new InvalidOperationException(
                    "No hay calendario activo del museo.");


            // 3. Validar los días de montaje y desmontaje.
            var montajeDisponible = RangoLogisticaDisponible(
                calendario,
                logisticaMuestraTemporal.FechaMontaje,
                logisticaMuestraTemporal.FechaMontajeHasta);

            var desmontajeDisponible = RangoLogisticaDisponible(
                calendario,
                logisticaMuestraTemporal.FechaDesmontaje,
                logisticaMuestraTemporal.FechaDesmontajeHasta);

            if (!montajeDisponible || !desmontajeDisponible)
                return false;


            // 4. Obtener las actividades existentes.
            var actividadesEnRango =
                await ObtenerActividadesConConflictoPotencial(
                    desde,
                    hasta,
                    muestraIdExcluir);


            // 5. Preparar las actividades para el motor.
            var existingActivities = _availabilityFactory.Create(
                actividadesEnRango.Cast<ActividadMuseoEntity>().ToList(),
                desde,
                hasta);


            // 6. Obtener las salas solicitadas.
            var salas = await _repositorioSalaMuseo
                .ObtenerSalasporIdsAsync(salasIds);


            // 7. Crear una muestra ficticia.
            var muestraFicticia = new MuestraTemporal(
                nombreResponsable: "Disponibilidad",
                telefonoResponsable: new Telefono("3511234567"),
                emailResponsable: new Email("disponibilidad@museo.com"),
                tituloPropuesta: "Muestra ficticia",
                logisticaMuestraTemporal: logisticaMuestraTemporal,
                requiereDifusion: false,
                salas: salas,
                fechaMuestra: horarioMuestra);


            // 8. Crear el candidato para el motor.
            var candidato = new CandidateEntry(
                Guid.NewGuid(),
                muestraFicticia,
                new List<TimeSlot>
                {
                    muestraFicticia.Horario
                }.AsReadOnly(),
                null,
                "MuestraCandidate");


            var candidatos = new List<CandidateEntry>
            {
                candidato
            };


            // 9. Crear el contexto de disponibilidad.
            var contexto = new AvailabilityContext
            {
                Start = desde,
                End = hasta,
                ExistingActivities = existingActivities,
                Metadata = new Dictionary<string, object>
                {
                    {
                        "tipoActividad",
                        TipoActividad.MuestraExposicionTemporal
                    },
                    {
                        "salasIds",
                        salasIds
                    }
                }
            };


            // 10. Validar disponibilidad con el motor.
            var resultados = await _engine.CheckManyAsync(
                contexto,
                candidatos);


            // 11. Devolver el resultado.
            return resultados.TryGetValue(
                candidato.Id,
                out var resultado)
                && resultado.IsOk;
        }


        private static bool RangoLogisticaDisponible(
            HorarioAperturaMuseo calendario,
            DateTime desde,
            DateTime hasta)
        {
            // Comprobar todos los días comprendidos en el período.
            for (var fecha = desde.Date;
                 fecha <= hasta.Date;
                 fecha = fecha.AddDays(1))
            {
                var franjasOperativas = calendario
                    .ObtenerFranjasOperativas(
                        DateOnly.FromDateTime(fecha));

                // Si el museo está cerrado ese día,
                // el período logístico no está disponible.
                if (!franjasOperativas.Any())
                    return false;
            }

            return true;
        }


        private async Task<List<ActividadMuseoEntity>>
            ObtenerActividadesConConflictoPotencial(
                DateTime desde,
                DateTime hasta,
                string? muestraIdExcluir = null)
        {
            var actividades = await _repositorioActividadMuseo
                .FindAllAsync(desde, hasta);

            // Excluir la muestra actual si se está modificando.
            if (!string.IsNullOrWhiteSpace(muestraIdExcluir))
            {
                actividades = actividades
                    .Where(actividad => actividad.Id != muestraIdExcluir)
                    .ToList();
            }

            return actividades;
        }
    }
}
