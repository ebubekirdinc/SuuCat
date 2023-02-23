namespace Shared.Interfaces
{
    public interface IOrderRequestFailedEvent
    {
        public int OrderId { get; set; }
        public string Reason { get; set; }
    }
}