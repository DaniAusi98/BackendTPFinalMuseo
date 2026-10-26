using Application.Eventos.ApplicationServices;
using Application.Eventos.Repositories;
using Core.Application;

namespace Application.Eventos.UseCases.Commands.ReenviarFormularioEvento
{
    internal class ReenviarFormularioEventoCommandHanlder(
        IRepositorioPropuestaEvento repositorioPropuestaEvento,
        IGenerarUrlFormularioEvento generarUrlFormularioEvento,
        IEnvioFormularioEventoEmailService envioFormularioEventoEmailService)
        : IRequestCommandHandler<ReenviarFormularioEventoCommand>
    {
        private readonly IRepositorioPropuestaEvento _repositorio =
            repositorioPropuestaEvento;

        private readonly IGenerarUrlFormularioEvento _generarUrl =
            generarUrlFormularioEvento;

        private readonly IEnvioFormularioEventoEmailService _envioEmail =
            envioFormularioEventoEmailService;

        public async Task Handle(
            ReenviarFormularioEventoCommand request,
            CancellationToken cancellationToken)
        {
            var propuesta = await _repositorio.FindOneAsync(
                request.IdEvento);

            if (propuesta is null)
                throw new KeyNotFoundException(
                    $"No se encontró la propuesta con ID {request.IdEvento}.");

            string urlFormulario = _generarUrl.GenerarUrlFormularioEvento(
                propuesta.Id,
                propuesta.TipoEvento);

            await _envioEmail.SendEnvioFormularioEmailAsync(
                propuesta.EmailResponsable.Valor,
                propuesta.TituloPropuesta,
                urlFormulario);
        }
    }
}