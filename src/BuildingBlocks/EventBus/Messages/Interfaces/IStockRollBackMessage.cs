using System.Collections.Generic;
using EventBus.Events;

namespace EventBus.Messages.Interfaces;

public interface IStockRollBackMessage
{
    public List<OrderItem> OrderItemList { get; set; }
}