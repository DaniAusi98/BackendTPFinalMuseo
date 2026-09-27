using Application.ApplicationMuseo.Constants;
using Application.Exceptions;
using Application.MuseumResources.Repositories;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Domain.Common.ValueObjets;

namespace Application.VisitaGrupal.UseCases.Comands.CrearVisitaAutoguiada
{
    internal sealed class CrearVisitaAutoguiadaHandler(ICommandQueryBus commandQueryBus,IRepositorioVisitaGrupalAutoguiada repositorioVisitaGrupalAutoguiada,IRepositorioSala repositorioSala, IRepositorioTematicas _tematicaRepository) : IRequestCommandHandler<CrearVisitaAutoguiadaCommand, string>
    {
        private readonly ICommandQueryBus _commandQueryBus = commandQueryBus ?? throw new ArgumentNullException(nameof(commandQueryBus));
        private readonly IRepositorioVisitaGrupalAutoguiada _repositorioVisitaGrupalAutoguiada = repositorioVisitaGrupalAutoguiada ?? throw new ArgumentNullException(nameof(repositorioVisitaGrupalAutoguiada));
        private readonly IRepositorioSala _repositorioSala = repositorioSala ?? throw new ArgumentNullException(nameof(repositorioSala));
        private readonly IRepositorioTematicas _tematicaRepository = _tematicaRepository ?? throw new ArgumentNullException(nameof(_tematicaRepository));
        public async Task<string> Handle(CrearVisitaAutoguiadaCommand request, CancellationToken cancellationToken)
        {
            var timeSlot = new TimeSlot(
               request.Inicio,
               request.Fin
           );
            // Temáticas opcionales
            var tematicas = request.TematicasIds.Any()
                ? await _tematicaRepository.GetByIdsAsync(request.TematicasIds)
                : [];

            if (tematicas.Count != request.TematicasIds.Count)
            {
                throw new BussinessException(
                    "Una o más temáticas no existen.");
            }

            // Salas opcionales
            var salas = request.SalasIds.Any()
                ? await _repositorioSala.ObtenerSalasporIdsAsync(request.SalasIds)
                : [];

            if (salas.Count != request.SalasIds.Count)
            {
                throw new BussinessException(
                    "Una o más salas no existen.");
            }

            var visita = new Domain.VisitasGrupales.Entities.VisitaGrupalAutoguiada(
                usuarioVisitanteId: request.UsuarioVisitanteId,
                institucion: request.Institucion,
                emailInstitucion: new Email(request.EmailInstitucion),
                paisInstitucion: request.PaisInstitucion,
                provinciaInstitucion: request.ProvinciaInstitucion,
                ciudadInstitucion: request.LocalidadInstitucion,
                descripcionDiversidad: request.DiversidadFuncionalDescripcion,
                observaciones: request.Observaciones,
                cantidadPersonas: request.CantidadPersonas,
                salas: salas,
                tematicas: tematicas,

                horario: timeSlot
            );
            try
            {
                object createdId = await _repositorioVisitaGrupalAutoguiada.AddAsync(visita);

                //await _domainBus.Publish(entity.To<DummyEntityCreated>(), cancellationToken);

                return createdId.ToString();
            }
            catch (Exception ex)
            {
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException);
            }
        }


    }
}
