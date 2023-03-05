using System;
using System.Collections.Generic;
using MassTransit;
using Shared.Events;

namespace Shared.Messages.Interfaces
{
    public interface ICompletePaymentMessage : CorrelatedBy<Guid>
    {
        public decimal TotalPrice { get; set; }
        public List<OrderItem> OrderItems { get; set; }

        public string CustomerId { get; set; }
    }
}