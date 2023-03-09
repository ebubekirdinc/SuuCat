using EventBus.Events.Interfaces;

namespace EventBus.Events;

public class OrderCompletedEvent : IOrderCompletedEvent
{
    public int OrderId { get; set; }
}