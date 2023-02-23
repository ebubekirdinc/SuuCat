using System.Collections.Generic;
using Shared.Events;

namespace Shared.Messages
{
    public interface IStockRollBackMessage
    {
        public List<OrderCreatedRequestEventItem> OrderItems { get; set; }
    }
}