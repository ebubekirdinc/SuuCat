using Shared.Events.Interfaces;

namespace Shared.Events
{
    public class OrderFailedEvent : IOrderFailedEvent
    {
        public int OrderId { get; set; }
        public string Reason { get; set; }
    }
}