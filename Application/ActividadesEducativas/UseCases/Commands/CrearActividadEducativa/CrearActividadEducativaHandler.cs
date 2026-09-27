using Application.ActividadesEducativas.Repositories;
using Application.ApplicationMuseo.Constants;
using Application.Exceptions;
using Application.MuseumResources.Repositories;
using Core.Application;
using Domain.ActividadesAreaEducacion.Entities;
using Domain.ActividadMuseo.Entities;
using Domain.Common.ValueObjets;

namespace Application.ActividadesEducativas.UseCases.Commands.CrearActividadEducativa
{
    internal sealed class CrearActividadEducativaHandler(
        IRepositorioActividadEducativa repositorioActividadEducativa,
        IRepositorioProyectoAreaEducacion repositorioProyecto,
        IRepositorioSala repositorioSala,
        IRepositorioRecurso repositorioRecurso)
        : IRequestCommandHandler<CrearActividadEducativaCommand, string>
    {
        private readonly IRepositorioActividadEducativa _repositorioActividadEducativa =
            repositorioActividadEducativa
            ?? throw new ArgumentNullException(nameof(repositorioActividadEducativa));

        private readonly IRepositorioProyectoAreaEducacion _repositorioProyecto =
            repositorioProyecto
            ?? throw new ArgumentNullException(nameof(repositorioProyecto));

        private readonly IRepositorioSala _repositorioSala =
            repositorioSala
            ?? throw new ArgumentNullException(nameof(repositorioSala));

        private readonly IRepositorioRecurso _repositorioRecurso =
            repositorioRecurso
            ?? throw new ArgumentNullException(nameof(repositorioRecurso));

        public async Task<string> Handle(
            CrearActividadEducativaCommand request,
            CancellationToken cancellationToken)
        {
            // Proyecto vinculado
            var proyecto = await _repositorioProyecto
                .FindOneAsync(request.ProyectoVinculadaId);

            if (proyecto == null)
            {
                throw new BussinessException(
                    "El proyecto vinculado no existe.");
            }

            // Salas
            var salas = await _repositorioSala
                .ObtenerSalasporIdsAsync(request.SalasIds);

            if (salas.Count != request.SalasIds.Count)
            {
                throw new BussinessException(
                    "Una o más salas no existen.");
            }

            // 2. Validar la existencia de los recursos
            var recursoIds = request.Recursos.Select(r => r.RecursoId).Distinct().ToList();
            var recursosDb = await _repositorioRecurso.FindAllAsync();
            var recursosSolicitados = recursosDb.Where(r => recursoIds.Contains(r.Id)).ToList();

            if (recursosSolicitados.Count != recursoIds.Count)
                throw new BussinessException("Uno o más recursos no existen.");

            var recursosAsignados = request.Recursos
                .Select(r => new RecursoAsignado(r.RecursoId, r.CantidadAsignada, request.Inicio, request.Fin))
                .ToList();

            var timeSlot = new TimeSlot(
                request.Inicio,
                request.Fin);

            var actividad = new ActividadEducativa(
                titulo: request.Titulo,
                tipoActividadEducativa: request.TipoActividadEducativa,
                proyectoVinculadaId: request.ProyectoVinculadaId,
                institucionVinculada: request.InstitucionVinculada,
                publicoObjetivo: request.PublicoObjetivo,
                observaciones: request.Observaciones,
                horario: timeSlot,
                salas: salas,
                requiereDifusion: request.RequiereDifusion,
                solicitaFlyer: request.SolicitarAsistenciaDifusion,
                cantidadAsistentes: request.CantidadAsistentes,
                recursos: recursosAsignados,
                urlImagenes: request.UrlImagenes,
                rrule: request.RRule
            );

            try
            {
                object createdId = await _repositorioActividadEducativa
                    .AddAsync(actividad);

                return createdId.ToString();
            }
            catch (Exception ex)
            {
                throw new BussinessException(
                    ApplicationConstants.PROCESS_EXECUTION_EXCEPTION,
                    ex.InnerException);
            }
        }
    }
}