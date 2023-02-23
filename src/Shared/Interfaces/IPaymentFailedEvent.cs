using MassTransit;
using System;
using System.Collections.Generic;
using Shared.Events;

namespace Shared.Interfaces
{
    public interface IPaymentFailedEvent : CorrelatedBy<Guid>
    {
        public string Reason { get; set; }
        public List<OrderCreatedRequestEventItem> OrderItems { get; set; }
    }
}