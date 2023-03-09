using System;
using MassTransit;

namespace EventBus.Events.Interfaces;

public interface IStockNotReservedEvent : CorrelatedBy<Guid>
{
    string Reason { get; set; }
}