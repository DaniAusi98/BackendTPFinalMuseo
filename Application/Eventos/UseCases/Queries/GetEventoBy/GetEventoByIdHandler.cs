using Application.Eventos.DataTransferObjets;
using Application.Eventos.Repositories;
using Core.Application;
using Core.Application.Mapping;
using Domain.Eventos.Entities;

namespace Application.Eventos.UseCases.Queries.GetEventoBy
{
    internal sealed class GetEventoByIdHandler(IRepositorioEvento repositorioEvento) : IRequestQueryHandler<GetEventoByIdQuery, EventoDto>
    {
        private readonly IRepositorioEvento repositorioEvento1 = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
        public async Task<EventoDto> Handle(GetEventoByIdQuery request, CancellationToken cancellationToken)
        {
            Evento evento = await repositorioEvento1.FindOneIncludeAsync(request.EventoId);
            return evento.To<EventoDto>();
        }
    }
}
