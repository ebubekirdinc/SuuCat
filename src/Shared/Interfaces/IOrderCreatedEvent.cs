using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Shared.Events;

namespace Shared.Interfaces
{
    public interface IOrderCreatedEvent : CorrelatedBy<Guid>
    {
        List<OrderCreatedRequestEventItem> OrderItems { get; set; }
    }
}