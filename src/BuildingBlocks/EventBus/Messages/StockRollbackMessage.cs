using System.Collections.Generic;
using EventBus.Events;
using EventBus.Messages.Interfaces;

namespace EventBus.Messages;

public class StockRollbackMessage : IStockRollBackMessage
{
    public List<OrderItem> OrderItemList { get; set; }
}