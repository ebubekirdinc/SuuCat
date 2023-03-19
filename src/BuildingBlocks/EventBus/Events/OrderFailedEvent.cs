using EventBus.Events.Interfaces;

namespace EventBus.Events;

public class OrderFailedEvent : IOrderFailedEvent
{
    public int OrderId { get; set; }
    public string CustomerId { get; set; }
    public string ErrorMessage { get; set; }
}