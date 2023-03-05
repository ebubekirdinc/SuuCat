using System;
using System.Collections.Generic;
using MassTransit;

namespace Shared.Events.Interfaces
{
    public interface IPaymentFailedEvent : CorrelatedBy<Guid>
    {
        public string Reason { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}