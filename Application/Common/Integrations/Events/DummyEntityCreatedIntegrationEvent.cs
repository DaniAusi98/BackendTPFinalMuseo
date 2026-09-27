using Core.Application;

namespace Application.ApplicationMuseo.Integrations.Events
{
    public class DummyEntityCreatedIntegrationEvent : IntegrationEvent
    {
        public DummyEntityCreatedIntegrationEvent()
        {
        }

        public DummyEntityCreatedIntegrationEvent(string eventType, string subject, object data) 
            : base(eventType, subject, data)
        {
        }

        public DummyEntityCreatedIntegrationEvent(Guid id, DateTime date, string eventType, string subject, object data) 
            : base(id, date, eventType, subject, data)
        {
        }
    }
}
