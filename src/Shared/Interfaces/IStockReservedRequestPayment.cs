using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Events;

namespace Shared.Interfaces
{
    public interface IStockReservedRequestPayment : CorrelatedBy<Guid>
    {
        public decimal TotalPrice { get; set; }
        public List<OrderCreatedRequestEventItem> OrderItems { get; set; }

        public string CustomerId { get; set; }
    }
}