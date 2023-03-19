using EventBus.Events.Interfaces;

namespace EventBus.Events;

public class StockReservationFailedEvent : IStockReservationFailedEvent
{
    public Guid CorrelationId { get; set; }
    public string ErrorMessage { get; set; }
}