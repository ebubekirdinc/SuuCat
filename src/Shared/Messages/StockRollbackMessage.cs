using System.Collections.Generic;
using Shared.Events;

namespace Shared.Messages
{
    public class StockRollbackMessage : IStockRollBackMessage
    {
        public List<OrderCreatedRequestEventItem> OrderItems { get; set; }
    }
}