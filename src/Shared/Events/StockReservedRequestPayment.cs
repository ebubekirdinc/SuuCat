using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public class StockReservedRequestPayment : IStockReservedRequestPayment
    {
        public StockReservedRequestPayment(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public decimal TotalPrice { get; set; }
        public List<OrderCreatedRequestEventItem> OrderItems { get; set; }

        public Guid CorrelationId { get; }
        public string CustomerId { get; set; }
    }
}