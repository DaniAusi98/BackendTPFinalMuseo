using Application.ApplicationMuseo.Constants;
using Application.Exceptions;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Domain.Common.ValueObjets;
using Domain.VisitasGrupales.Entities.GrupalGuiada;

namespace Application.VisitaGrupal.UseCases.VisitaGuiadaUC.Commands.ReprogramarVisitaGrupal.ReprogramarVisitaGuiada
{
    internal sealed class VisitaReprogramadaHandler(IRepositorioVisitaGuiada repositorioVisitaGuiada, IRepositorioTematicas repositorioTematicas, IReporsitorioVisitaReprogramada reporsitorioVisitaReprogramada) : IRequestCommandHandler<ReprogramarCommand, string>
    {
        private readonly IRepositorioVisitaGuiada _repositorioVisitaGuiada = repositorioVisitaGuiada ?? throw new ArgumentNullException(nameof(repositorioVisitaGuiada));
        private readonly IRepositorioTematicas _repositorioTematicas = repositorioTematicas ?? throw new ArgumentNullException(nameof(repositorioTematicas));
        private readonly IReporsitorioVisitaReprogramada _reporsitorioVisitaReprogramada = reporsitorioVisitaReprogramada ?? throw new ArgumentNullException(nameof(reporsitorioVisitaReprogramada));
        public async Task<string> Handle(ReprogramarCommand request, CancellationToken cancellationToken)
        {
            var visitaReprogramada = await _repositorioVisitaGuiada.FindOneAsync(request.VisitaReprogramadaId) ?? throw new Exception("Visita no encontrada");


            var timeSlot = new TimeSlot(
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
            var visita = new VisitaGrupalGuiada(
                usuarioVisitanteId: request.UsuarioVisitanteId,
                nombreSolicitante: request.NombreSolicitante,
                apellidoSolicitante: request.ApellidoSolicitante,
                emailSolicitante: new Email(request.EmailSolicitante),
                telefonoSolicitante: new Telefono(request.TelefonoSolicitante),
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
                visitaReprogramada.CambiarEstadoAReprogramada();

                _repositorioVisitaGuiada.Update(
                    request.VisitaReprogramadaId,
                    visitaReprogramada);

                var createdId = await _repositorioVisitaGuiada.AddAsync(visita);

                var registroReprogramacion = new VisitaReprogramada(
                    visitaReprogramadaId: request.VisitaReprogramadaId,
                    nuevaVisitaId: createdId.ToString()!,
                    horarioReprogramado: visitaReprogramada.Horario,
                    institucion: visita.Institucion,
                    nivelEducativo: visita.NivelEducativo,
                    anioGrado: visita.AnioGrado,
                    paisInstitucion: visita.PaisInstitucion,
                    provinciaInstitucion: visita.ProvinciaInstitucion,
                    localidadInstitucion: visita.LocalidadInstitucion,
                    estado: visita.Estado,
                    cantidadPersonas: visita.CantidadPersonas,
                    nuevoHorario: visita.Horario
                );

                var registroReprogramacionId = await _reporsitorioVisitaReprogramada.AddAsync(registroReprogramacion);

                return createdId.ToString()!;
            }
            catch (Exception ex)
            {
                throw new BussinessException(
                    ApplicationConstants.PROCESS_EXECUTION_EXCEPTION,
                    ex.InnerException ?? ex);
            }

        }
    }
}
