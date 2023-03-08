using System;
using MassTransit;

namespace EventBus.Events.Interfaces
{
    public interface IPaymentCompletedEvent : CorrelatedBy<Guid>
    {
    }
}