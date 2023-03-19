using MassTransit;

namespace EventBus.Events.Interfaces;

public interface IStockReservationFailedEvent : CorrelatedBy<Guid>
{
    string ErrorMessage { get; set; }
}