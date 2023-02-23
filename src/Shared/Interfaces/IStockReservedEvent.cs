using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Events;

namespace Shared.Interfaces
{
    public interface IStockReservedEvent : CorrelatedBy<Guid>
    {
        List<OrderCreatedRequestEventItem> OrderItems { get; set; }
    }
}