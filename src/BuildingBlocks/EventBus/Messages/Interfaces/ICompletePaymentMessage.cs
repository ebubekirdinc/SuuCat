using System;
using System.Collections.Generic;
using EventBus.Events;
using MassTransit;

namespace EventBus.Messages.Interfaces;

public interface ICompletePaymentMessage : CorrelatedBy<Guid>
{
    public string CustomerId { get; set; }
    public decimal TotalPrice { get; set; }
    public List<OrderItem> OrderItemList { get; set; }

}