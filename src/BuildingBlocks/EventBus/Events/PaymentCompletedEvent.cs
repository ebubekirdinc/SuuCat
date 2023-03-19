using EventBus.Events.Interfaces;

namespace EventBus.Events;

public class PaymentCompletedEvent : IPaymentCompletedEvent
{
    public Guid CorrelationId { get; set; }
}