using System;
using System.Collections.Generic;
using MassTransit;

namespace EventBus.Events.Interfaces
{
    public interface IOrderCreatedEvent : CorrelatedBy<Guid>
    {
        List<OrderItem> OrderItems { get; set; }
    }
}