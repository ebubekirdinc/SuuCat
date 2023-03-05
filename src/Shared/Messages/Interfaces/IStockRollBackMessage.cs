using System.Collections.Generic;
using Shared.Events;

namespace Shared.Messages.Interfaces
{
    public interface IStockRollBackMessage
    {
        public List<OrderItem> OrderItems { get; set; }
    }
}