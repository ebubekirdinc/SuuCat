using MassTransit;
using System;

namespace Shared.Interfaces
{
    public interface IStockNotReservedEvent : CorrelatedBy<Guid>
    {
        string Reason { get; set; }
    }
}