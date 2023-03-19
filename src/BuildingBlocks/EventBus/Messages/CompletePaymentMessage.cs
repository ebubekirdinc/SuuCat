using EventBus.Events;
using EventBus.Messages.Interfaces;

namespace EventBus.Messages;

public class CompletePaymentMessage : ICompletePaymentMessage
{
    public Guid CorrelationId { get; set; }
    public string CustomerId { get; set; }
    public decimal TotalPrice { get; set; }
    public List<OrderItem> OrderItemList { get; set; }

}