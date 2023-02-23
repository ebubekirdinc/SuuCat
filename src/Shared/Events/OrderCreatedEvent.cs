using System;
using System.Collections.Generic;
using Shared.Interfaces;

namespace Shared.Events
{
    public class OrderCreatedEvent : IOrderCreatedEvent
    {
        public OrderCreatedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public List<OrderCreatedRequestEventItem> OrderItems { get; set; }

        public Guid CorrelationId { get; }
    }
}