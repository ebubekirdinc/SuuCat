using System;
using MassTransit;

namespace Shared.Events.Interfaces
{
    public interface IPaymentCompletedEvent : CorrelatedBy<Guid>
    {
    }
}