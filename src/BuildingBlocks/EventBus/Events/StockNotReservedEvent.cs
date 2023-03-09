using System;
using EventBus.Events.Interfaces;

namespace EventBus.Events;

public class StockNotReservedEvent : IStockNotReservedEvent
{
    public StockNotReservedEvent(Guid correlationId)
    {
        CorrelationId = correlationId;
    }

    public string Reason { get; set; }

    public Guid CorrelationId { get; }
}