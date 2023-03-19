using System.Collections.Generic;
using EventBus.Events;
using EventBus.Messages.Interfaces;

namespace EventBus.Messages;

public class CreateOrderMessage : ICreateOrderMessage
{
    public CreateOrderMessage()
    {
        OrderItemList = new List<OrderItem>();
    }

    public int OrderId { get; set; }
    public string CustomerId { get; set; }
    public string PaymentAccountId { get; set; }
    public decimal TotalPrice { get; set; }

    public List<OrderItem> OrderItemList { get; set; }
}