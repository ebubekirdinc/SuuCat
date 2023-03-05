using System.Collections.Generic;
using Shared.Events;

namespace Shared.Messages.Interfaces
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