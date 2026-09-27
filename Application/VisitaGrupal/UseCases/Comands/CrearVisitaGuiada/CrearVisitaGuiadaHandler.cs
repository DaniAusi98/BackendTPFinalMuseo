using Application.ApplicationMuseo.Constants;
using Application.Exceptions;
using Application.VisitaGrupal.DomainEvents;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Core.Application.Mapping;
using Domain.Common.ValueObjets;
using Domain.VisitasGrupales.Entities;
using Domain.VisitasGrupales.Entities.GrupalGuiada;
using Domain.VisitasGrupales.Enums;


namespace Application.VisitaGrupal.UseCases.Comands.CrearVisitaGuiada
{
    internal sealed class CrearVisitaGuiadaHandler(ICommandQueryBus domainBus, IRepositorioVisitaGuiada repositorioVisitaGuiada,IRepositorioTematicas tematicaRepository) : IRequestCommandHandler<CrearVisitaGuiadaCommand, string>
    {
        private readonly ICommandQueryBus _domainBus = domainBus ?? throw new ArgumentNullException(nameof(domainBus));
        private readonly IRepositorioVisitaGuiada _repositorioVisitaGuiada = repositorioVisitaGuiada ?? throw new ArgumentNullException(nameof(repositorioVisitaGuiada));
        private readonly IRepositorioTematicas _tematicaRepository = tematicaRepository ?? throw new ArgumentNullException(nameof(tematicaRepository));

        public async Task<string> Handle(CrearVisitaGuiadaCommand request, CancellationToken cancellationToken)
        {

            var timeSlot = new TimeSlot(
                request.Inicio,
                request.Fin
            );

            var tematicas = await _tematicaRepository
                .GetByIdsAsync(request.TematicasIds);

            if (tematicas.Count != request.TematicasIds.Count)
            {
                throw new BussinessException(
                    "Una o más temáticas no existen.");
            }
            var salas = tematicas
                .SelectMany(t => t.Salas)
                .DistinctBy(s => s.Id)
                .ToList();

            var visita = new VisitaGrupalGuiada(
                usuarioVisitanteId: request.UsuarioVisitanteId,
                nivelEducativo: request.NivelEducativo,
                anioGrado: request.AnioGrado,
                cantidadPersonas: request.CantidadPersonas,
                institucion: request.Institucion,
                emailInstitucion: new Email(request.EmailInstitucion),
                telefonoInstitucion: new Telefono(request.TelefonoInstitucion),
                paisInstitucion: request.PaisInstitucion,
                provinciaInstitucion: request.ProvinciaInstitucion,
                ciudadInstitucion: request.LocalidadInstitucion,
                descripcionDiversidad: request.DiversidadFuncionalDescripcion,
                motivoVisita: request.MotivoRelacionVisita,
                observaciones: request.Observaciones,
                horario: timeSlot,
                tematicas: tematicas,
                salas: salas
            );
          
            try
            {
                object createdId = await _repositorioVisitaGuiada.AddAsync(visita);

                await _domainBus.Publish(visita.To<VisitaGuiadaCreated>(), cancellationToken);

                return createdId.ToString();
            }
            catch (Exception ex)
            {
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException);
            }

        }
    }
}

