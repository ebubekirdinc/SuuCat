using System;
using System.Collections.Generic;
using MassTransit;

namespace Shared.Events.Interfaces
{
    public interface IStockReservedEvent : CorrelatedBy<Guid>
    {
        List<OrderItem> OrderItems { get; set; }
    }
}