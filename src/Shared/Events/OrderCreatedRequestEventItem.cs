namespace Shared.Events
{
    public class OrderCreatedRequestEventItem
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
    }
}