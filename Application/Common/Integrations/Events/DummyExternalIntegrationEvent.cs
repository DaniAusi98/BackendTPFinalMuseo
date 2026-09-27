using Core.Application;

namespace Application.ApplicationMuseo.Integrations.Events
{
    public class DummyExternalIntegrationEvent : IntegrationEvent
    {
        public DummyExternalIntegrationEvent()
        {
        }

        public DummyExternalIntegrationEvent(string eventType, string subject, object data)
            : base(eventType, subject, data)
        {
        }

        public DummyExternalIntegrationEvent(Guid id, DateTime date, string eventType, string subject, object data)
            : base(id, date, eventType, subject, data)
        {
        }
    }
}
