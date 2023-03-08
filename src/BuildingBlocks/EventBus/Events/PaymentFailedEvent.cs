using System;
using System.Collections.Generic;
using EventBus.Events.Interfaces;

namespace EventBus.Events
{
    public class PaymentFailedEvent : IPaymentFailedEvent
    {
        public PaymentFailedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public string Reason { get; set; }
        public List<OrderItem> OrderItems { get; set; }

        public Guid CorrelationId { get; }
    }
}