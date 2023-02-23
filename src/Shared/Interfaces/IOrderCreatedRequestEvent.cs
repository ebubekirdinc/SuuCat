using System.Collections.Generic;
using Shared.Events;

namespace Shared.Interfaces
{
    public interface IOrderCreatedRequestEvent
    {
        public int OrderId { get; set; }
        public string CustomerId { get; set; }
        public string PaymentAccountId { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderCreatedRequestEventItem> OrderItems { get; set; }
    }
}