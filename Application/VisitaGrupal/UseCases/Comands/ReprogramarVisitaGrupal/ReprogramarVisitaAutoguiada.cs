using Application.Exceptions;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Domain.VisitasGrupales.Entities;


namespace Application.VisitaGrupal.UseCases.Comands.ReprogramarVisitaGrupal
{
    internal sealed class ReprogramarVisitaAutoguiada(IRepositorioVisitaGrupalAutoguiada repositorioVisitaGrupalAutoguiada ,IRepositorioTematicas repositorioTematicas) : IRequestCommandHandler<ReprogramarVisitaAutoguiadaCommand, string>
    {
        private readonly IRepositorioVisitaGrupalAutoguiada _repositorioVisitaGrupalAutoguiada = repositorioVisitaGrupalAutoguiada ?? throw new ArgumentNullException(nameof(repositorioVisitaGrupalAutoguiada));
        private readonly IRepositorioTematicas _repositorioTematicas = repositorioTematicas ?? throw new ArgumentNullException(nameof(repositorioTematicas));
        public async Task<string> Handle(ReprogramarVisitaAutoguiadaCommand request, CancellationToken cancellationToken)
        {
            var visitaReprogramada = await _repositorioVisitaGrupalAutoguiada.FindOneAsync(request.VisitaReprogramadaId) ?? throw new Exception("Visita no encontrada");
            var timeSlot = new Domain.Common.ValueObjets.TimeSlot(
               request.Inicio,
               request.Fin
           );
            var tematicas = await _repositorioTematicas
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
            var visita = new VisitaGrupalAutoguiada(
                usuarioVisitanteId: request.UsuarioVisitanteId,
                institucion: request.Institucion,
                emailInstitucion: new Domain.Common.ValueObjets.Email(request.EmailInstitucion),
                paisInstitucion: request.PaisInstitucion,
                provinciaInstitucion: request.ProvinciaInstitucion,
                ciudadInstitucion: request.LocalidadInstitucion,
                descripcionDiversidad: request.DiversidadFuncionalDescripcion,
                observaciones: request.Observaciones,
                cantidadPersonas: request.CantidadPersonas,
                horario: timeSlot,
                tematicas: tematicas,
                salas: salas
            );
            try
            {
                visitaReprogramada.CambiarEstadoAReprogramada();
                _repositorioVisitaGrupalAutoguiada.Update(request.VisitaReprogramadaId, visitaReprogramada);
                object createdId = await _repositorioVisitaGrupalAutoguiada.AddAsync(visita);

                //await _domainBus.Publish(visita.To<VisitaAutoguiadaReprogramada>(), cancellationToken);

                return createdId.ToString();
            }
            catch (Exception ex)
            {
                throw new BussinessException("Error al reprogramar la visita autoguiada.", ex);
            }
        }
    }
}
