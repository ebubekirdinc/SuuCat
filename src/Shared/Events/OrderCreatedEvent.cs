using System;
using System.Collections.Generic;
using Shared.Events.Interfaces;

namespace Shared.Events
{
    public class OrderCreatedEvent : IOrderCreatedEvent
    {
        public OrderCreatedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public List<OrderItem> OrderItems { get; set; }

        public Guid CorrelationId { get; }
    }
}