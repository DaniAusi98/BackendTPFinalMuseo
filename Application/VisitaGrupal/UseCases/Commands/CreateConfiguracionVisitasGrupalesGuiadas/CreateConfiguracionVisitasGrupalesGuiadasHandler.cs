using Application.ActividadMuseo.Repositories;
using Application.Exceptions;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Domain.Common.ValueObjets;
using Domain.VisitasGrupales.Entities;
using Domain.VisitasGrupales.Entities.GrupalGuiada;
using Domain.VisitasGrupales.ValueObjects;
using MediatR;

namespace Application.VisitaGrupal.UseCases.Commands.CreateConfiguracionVisitasGrupalesGuiadas
{
    internal sealed class CreateConfiguracionVisitasGrupalesGuiadasHandler : IRequestCommandHandler<CreateConfiguracionVisitasGrupalesGuiadasCommand, string>
    {
        private readonly IRepositorioConfiguracionVisitasGrupalesGuiadas _repositorio;
        private readonly IRepositorioCalendarioMuseo _repositorioCalendario;

        public CreateConfiguracionVisitasGrupalesGuiadasHandler(
            IRepositorioConfiguracionVisitasGrupalesGuiadas repositorio,
            IRepositorioCalendarioMuseo repositorioCalendario)
        {
            _repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            _repositorioCalendario = repositorioCalendario ?? throw new ArgumentNullException(nameof(repositorioCalendario));
        }

        public async Task<string> Handle(CreateConfiguracionVisitasGrupalesGuiadasCommand request, CancellationToken cancellationToken)
        {
            // Obtener el calendario del museo para validar
            var calendario = await _repositorioCalendario.ObtenerCalendarioActivoAsync()
                ?? throw new EntityDoesNotExistException("No se encontró el calendario del museo");

            var diasDisponibles = new DiasLaboralesMuseo(request.DiasDisponibles);

            var turnos = request.Turnos
                .Select(t => new TurnoVisitaGuiada(t.HoraInicio, t.HoraFin))
                .ToList();

            var configuracion = new ConfiguracionVisitasGrupalesGuiadas(
                request.MinGuiasParaCapacidadCompleta,
                request.CapacidadPorGuia,
                request.CapacidadMaximaPorTurno,
                diasDisponibles,
                turnos
            );

            // Validar días contra el calendario del museo
            configuracion.ActualizarDiasDisponibles(diasDisponibles, calendario);

            await _repositorio.AddAsync(configuracion);
            return configuracion.Id;
        }

       
    }
}