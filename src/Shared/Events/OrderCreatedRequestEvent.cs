using System.Collections.Generic;
using Shared.Interfaces;

namespace Shared.Events
{
    public class OrderCreatedRequestEvent : IOrderCreatedRequestEvent
    {
        public OrderCreatedRequestEvent()
        {
            OrderItems = new List<OrderCreatedRequestEventItem>();
        }
        public int OrderId { get; set; }
        public string CustomerId { get; set; }
        public string PaymentAccountId { get; set; }
        public decimal TotalPrice { get; set; }
        
        public List<OrderCreatedRequestEventItem> OrderItems { get; set; }
    }
}