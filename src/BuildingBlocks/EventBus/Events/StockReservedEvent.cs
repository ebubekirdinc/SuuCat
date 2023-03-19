using EventBus.Events.Interfaces;

namespace EventBus.Events;

public class StockReservedEvent : IStockReservedEvent
{
    public Guid CorrelationId { get; set; }
    public List<OrderItem> OrderItemList { get; set; }

}