using Application.Eventos.ApplicationServices;
using Application.Eventos.Repositories;
using Application.Exceptions;
using Core.Application;

namespace Application.Eventos.UseCases.Commands.ReenviarFormularioEvento
{
    internal sealed class ReenviarFormularioEventoCommandHandler(
        IRepositorioPropuestaEvento repositorioPropuestaEvento,
        IGenerarUrlFormularioEvento generarUrlFormularioEvento,
        IEnvioFormularioEventoEmailService envioFormularioEventoEmailService
    ) : IRequestCommandHandler<ReenviarFormularioEventoCommand>
    {
        private readonly IRepositorioPropuestaEvento _repositorioPropuestaEvento =
            repositorioPropuestaEvento
            ?? throw new ArgumentNullException(nameof(repositorioPropuestaEvento));

        private readonly IGenerarUrlFormularioEvento _generarUrlFormularioEvento =
            generarUrlFormularioEvento
            ?? throw new ArgumentNullException(nameof(generarUrlFormularioEvento));

        private readonly IEnvioFormularioEventoEmailService _envioFormularioEventoEmailService =
            envioFormularioEventoEmailService
            ?? throw new ArgumentNullException(nameof(envioFormularioEventoEmailService));

        public async Task Handle(
            ReenviarFormularioEventoCommand request,
            CancellationToken cancellationToken)
        {
            // Buscar la propuesta asociada al evento.
            var propuesta = await _repositorioPropuestaEvento.FindOneAsync(request.IdPropuesta);

            if (propuesta is null)
            {
                throw new EntityDoesNotExistException(
                    $"No se encontró la propuesta con ID {request.IdPropuesta}.");
            }

            var evento = propuesta.Evento;

            // Generar la URL con el ID del evento.
            string urlFormulario = _generarUrlFormularioEvento
                .GenerarUrlFormularioEvento(evento.Id, evento.TipoEvento);

            // Enviar el formulario al responsable.
            await _envioFormularioEventoEmailService
                .SendEnvioFormularioEmailAsync(
                    evento.EmailResponsable.Valor,
                    evento.Titulo,
                    urlFormulario);

            // Actualizar la propuesta únicamente si el envío fue exitoso. 
            propuesta.RegistrarEnvioFormulario();

            _repositorioPropuestaEvento.Update(propuesta.Id, propuesta);
        }
    }
}
