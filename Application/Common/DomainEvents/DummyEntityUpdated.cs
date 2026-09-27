using Core.Application;

using static Domain.Common.Enums.Enums;

namespace Application.ApplicationMuseo.DomainEvents
{
    internal sealed class DummyEntityUpdated : DomainEvent
    {
        public int DummyIdProperty { get; set; }
        public string DummyPropertyOne { get; set; }
        public DummyValues DummyPropertyTwo { get; set; }
    }
}
