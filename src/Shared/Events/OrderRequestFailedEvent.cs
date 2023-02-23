using Shared.Interfaces; 

namespace Shared.Events
{
    public class OrderRequestFailedEvent : IOrderRequestFailedEvent
    {
        public int OrderId { get; set; }
        public string Reason { get; set; }
    }
}