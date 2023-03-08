using System.Collections.Generic;
using EventBus.Events;

namespace EventBus.Messages.Interfaces
{
    public interface ICreateOrderMessage
    {
        public int OrderId { get; set; }
        public string CustomerId { get; set; }
        public string PaymentAccountId { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}