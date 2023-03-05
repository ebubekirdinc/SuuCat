using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Events.Interfaces;

namespace Shared.Events
{
    public class OrderCompletedEvent : IOrderCompletedEvent
    {
        public int OrderId { get; set; }
    }
}