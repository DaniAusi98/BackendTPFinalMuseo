using Application.ApplicationMuseo.Constants;
using Application.Exceptions;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Domain.Common.ValueObjets;
using Domain.VisitasGrupales.Entities.GrupalAutoguiada;

namespace Application.VisitaGrupal.UseCases.VisitaGuiadaUC.Commands.ReprogramarVisitaGrupal.ReprogramarAutoguiada
{
    internal sealed class ReprogramarVisitaAutoguiadaHandler(
        IRepositorioVisitaGrupalAutoguiada repositorioVisitaGrupalAutoguiada,
        IRepositorioTematicas repositorioTematicas,
        IRepositorioAutoguiadaReprogramada repositorioAutoguiadaReprogramada)
        : IRequestCommandHandler<ReprogramarVisitaAutoguiadaCommand, string>
    {
        private readonly IRepositorioVisitaGrupalAutoguiada _repositorioVisitaGrupalAutoguiada =
            repositorioVisitaGrupalAutoguiada
            ?? throw new ArgumentNullException(nameof(repositorioVisitaGrupalAutoguiada));

        private readonly IRepositorioTematicas _repositorioTematicas =
            repositorioTematicas
            ?? throw new ArgumentNullException(nameof(repositorioTematicas));

        private readonly IRepositorioAutoguiadaReprogramada _repositorioAutoguiadaReprogramada =
            repositorioAutoguiadaReprogramada
            ?? throw new ArgumentNullException(nameof(repositorioAutoguiadaReprogramada));

        public async Task<string> Handle(
            ReprogramarVisitaAutoguiadaCommand request,
            CancellationToken cancellationToken)
        {
            var visitaOriginal =
                await _repositorioVisitaGrupalAutoguiada.FindOneAsync(
                    request.VisitaReprogramadaId)
                ?? throw new Exception("Visita no encontrada.");

            var timeSlot = new TimeSlot(request.Inicio, request.Fin);

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

            var nuevaVisita = new VisitaGrupalAutoguiada(
                usuarioVisitanteId: request.UsuarioVisitanteId,
                nombreSolicitante: request.NombreSolicitante,
                apellidoSolicitante: request.ApellidoSolicitante,
                emailSolicitante: new Email(request.EmailSolicitante),
                telefonoSolicitante: new Telefono(request.TelefonoSolicitante),
                cantidadPersonas: request.CantidadPersonas,
                institucion: request.Institucion,
                emailInstitucion: new Email(request.EmailInstitucion),
                paisInstitucion: request.PaisInstitucion,
                provinciaInstitucion: request.ProvinciaInstitucion,
                ciudadInstitucion: request.LocalidadInstitucion,
                descripcionDiversidad: request.DiversidadFuncionalDescripcion,
                observaciones: request.Observaciones,
                horario: timeSlot,
                tematicas: tematicas,
                salas: salas
            );

            try
            {
                visitaOriginal.CambiarEstadoAReprogramada();

                _repositorioVisitaGrupalAutoguiada.Update(
                    request.VisitaReprogramadaId,
                    visitaOriginal);

                var nuevaVisitaId =
                    await _repositorioVisitaGrupalAutoguiada.AddAsync(nuevaVisita);

                var registroReprogramacion = new VisitaAutoguiadaReprogramada(
                    visitaReprogramadaId: request.VisitaReprogramadaId,
                    nuevaVisitaId: nuevaVisitaId.ToString()!,
                    horarioReprogramado: visitaOriginal.Horario,
                    institucion: nuevaVisita.Institucion,
                    paisInstitucion: nuevaVisita.PaisInstitucion,
                    provinciaInstitucion: nuevaVisita.ProvinciaInstitucion,
                    localidadInstitucion: nuevaVisita.LocalidadInstitucion,
                    estado: nuevaVisita.Estado,
                    cantidadPersonas: nuevaVisita.CantidadPersonas,
                    nuevoHorario: nuevaVisita.Horario
                );

                await _repositorioAutoguiadaReprogramada.AddAsync(
                    registroReprogramacion);

                return nuevaVisitaId.ToString()!;
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

