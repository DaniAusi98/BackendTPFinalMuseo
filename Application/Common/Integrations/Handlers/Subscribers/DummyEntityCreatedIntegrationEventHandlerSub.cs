using Application.ApplicationMuseo.Integrations.Events;
using Core.Application;

namespace Application.ApplicationMuseo.Integrations.Handlers.Subscribers
{
    public class DummyEntityCreatedIntegrationEventHandlerSub : IIntegrationEventHandler<DummyEntityCreatedIntegrationEvent>
    {
        public Task Handle(DummyEntityCreatedIntegrationEvent @event)
        {
            return Task.CompletedTask;
        }
    }
}
