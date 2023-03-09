using System;
using System.Collections.Generic;
using EventBus.Events;
using EventBus.Messages.Interfaces;

namespace EventBus.Messages;

public class CompletePaymentMessage : ICompletePaymentMessage
{
    public CompletePaymentMessage(Guid correlationId)
    {
        CorrelationId = correlationId;
    }

    public decimal TotalPrice { get; set; }
    public List<OrderItem> OrderItems { get; set; }

    public Guid CorrelationId { get; }
    public string CustomerId { get; set; }
}