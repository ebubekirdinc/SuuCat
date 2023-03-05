using System.Collections.Generic;
using Shared.Events;
using Shared.Messages.Interfaces;

namespace Shared.Messages
{
    public class StockRollbackMessage : IStockRollBackMessage
    {
        public List<OrderItem> OrderItems { get; set; }
    }
}