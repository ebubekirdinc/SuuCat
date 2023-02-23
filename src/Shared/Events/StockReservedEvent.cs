using System;
using System.Collections.Generic;
using Shared.Interfaces;

namespace Shared.Events
{
    public class StockReservedEvent : IStockReservedEvent
    {
        public StockReservedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public List<OrderCreatedRequestEventItem> OrderItems { get; set; }

        public Guid CorrelationId { get; }
    }
}